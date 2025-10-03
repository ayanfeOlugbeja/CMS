using CMS.Data;
using CMS.DTOs;
using CMS.Entities;
using CMS.Helpers;
using CMS.Repositories.Interfaces;
using CMS.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMS.Repositories.Services
{
    public class UserAccountRepository : IUserAccount
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly JwtSection _jwtConfig;

        public UserAccountRepository(AppDbContext context, IEmailService emailService, IOptions<JwtSection> jwtConfig)
        {
            _context = context;
            _emailService = emailService;
            _jwtConfig = jwtConfig.Value;
        }

        //  Registration
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user is null) return new GeneralResponse(false, "Model is empty");

            var existing = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (existing != null)
                return new GeneralResponse(false, "User is registered already");

            // Generate OTP (6-digit random number)
            var otp = new Random().Next(100000, 999999).ToString();

            var applicationUser = new ApplicationUser()
            {
                Fullname = user.Fullname,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                IsEmailVerified = false,
                EmailVerificationCode = otp,
                OtpExpiryTime = DateTime.UtcNow.AddMinutes(10)
            };

            _context.ApplicationUsers.Add(applicationUser);
            await _context.SaveChangesAsync();

            // Assign roles
            var checkAdminRole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Constants.Admin);
            if (checkAdminRole is null)
            {
                var adminRole = new SystemRole { Name = Constants.Admin };
                _context.SystemRoles.Add(adminRole);
                await _context.SaveChangesAsync();

                _context.UserRoles.Add(new UserRole { RoleId = adminRole.Id, UserId = applicationUser.Id });
            }
            else
            {
                var checkUserRole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Constants.User);
                if (checkUserRole is null)
                {
                    var userRole = new SystemRole { Name = Constants.User };
                    _context.SystemRoles.Add(userRole);
                    await _context.SaveChangesAsync();

                    _context.UserRoles.Add(new UserRole { RoleId = userRole.Id, UserId = applicationUser.Id });
                }
                else
                {
                    _context.UserRoles.Add(new UserRole { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
                }
            }

            await _context.SaveChangesAsync();

            // Send OTP via email
            _emailService.SendEmail(applicationUser.Email, "Your Verification Code",
                $"Hello {applicationUser.Fullname},\n\nYour OTP is: {otp}\n\nThis code will expire in 10 minutes.");

            return new GeneralResponse(true, "Account created! Please verify your email using the OTP.");
        }

        //  OTP verification
        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null || user.EmailVerificationCode != otp || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return false; // invalid or expired
            }

            user.IsEmailVerified = true;
            user.EmailVerificationCode = null;
            user.OtpExpiryTime = null;

            await _context.SaveChangesAsync();
            return true;
        }

        //  Login
        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var existing = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (existing == null)
            {
                return new LoginResponse { Flag = false, Message = "Invalid email or password" };
            }

            // Check if verified
            if (!existing.IsEmailVerified)
            {
                return new LoginResponse { Flag = false, Message = "Email not verified. Please verify OTP." };
            }

            // Validate password
            if (!BCrypt.Net.BCrypt.Verify(user.Password, existing.Password))
            {
                return new LoginResponse { Flag = false, Message = "Invalid email or password" };
            }

            // Fetch user roles
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == existing.Id)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            // Build claims with roles
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, existing.Id.ToString()),
        new Claim(ClaimTypes.Email, existing.Email),
        new Claim(ClaimTypes.Name, existing.Fullname ?? "")
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponse
            {
                Flag = true,
                Message = "Login successful",
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires ?? DateTime.UtcNow.AddHours(2)
            };
        }
    }
}
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

            // Send success email after verification
            _emailService.SendEmail(
                user.Email,
                "Registration Successful",
                $"Hello {user.Fullname}, your registration on CMS was successful!"
            );

            return true;
        }

        public async Task<bool> ResendOtpAsync(string email)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return false;

            var newOtp = new Random().Next(100000, 999999).ToString();
            user.EmailVerificationCode = newOtp;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Your New Verification Code",
                $"Hello {user.Fullname},\n\nYour new OTP is: {newOtp}\n\nThis code will expire in 10 minutes.");

            return true;
        }

        // Change Password
        public async Task<GeneralResponse> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
                return new GeneralResponse(false, "User not found.");

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
                return new GeneralResponse(false, "Current password is incorrect.");


            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();


            _emailService.SendEmail(user.Email, "Password Changed Successfully",
                $"Hi {user.Fullname},\n\nYour CMS account password was changed successfully.\nIf you didn’t request this, please contact support immediately.");

            return new GeneralResponse(true, "Password changed successfully.");
        }

        //  Forgot Password
        public async Task<GeneralResponse> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return new GeneralResponse(false, "User not found.");

            // Generate OTP for reset (6-digit code)
            var otp = new Random().Next(100000, 999999).ToString();

            // You could also create a token if you want reset via link
            var token = Guid.NewGuid().ToString("N");

            user.PasswordResetOtp = otp;
            user.PasswordResetToken = token;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(15);

            await _context.SaveChangesAsync();

            // Send both OTP and link via email
            var resetLink = $"https://yourfrontend.com/reset-password?token={token}&email={user.Email}";
            var body = $@"
        Hello {user.Fullname},<br><br>
        You requested to reset your password.<br><br>
        <b>OTP:</b> {otp} (valid for 15 minutes)<br><br>
        Or click this link to reset via browser:<br>
        <a href='{resetLink}'>Reset Password</a><br><br>
        If you did not request this, please ignore this email.";

            _emailService.SendEmail(user.Email, "Password Reset Request", body);

            return new GeneralResponse(true, "Password reset link and OTP sent to your email.");
        }



        public async Task<GeneralResponse> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return new GeneralResponse(false, "User not found.");

            // Validate OTP or token
            if ((user.PasswordResetOtp != model.OtpOrToken && user.PasswordResetToken != model.OtpOrToken)
                || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return new GeneralResponse(false, "Invalid or expired reset code/link.");
            }

            // Update password
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            // Clear reset data
            user.PasswordResetOtp = null;
            user.PasswordResetToken = null;
            user.OtpExpiryTime = null;

            await _context.SaveChangesAsync();

            // Notify user
            _emailService.SendEmail(user.Email, "Password Reset Successful",
                $"Hello {user.Fullname},\n\nYour password was successfully reset.");

            return new GeneralResponse(true, "Password reset successful.");
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
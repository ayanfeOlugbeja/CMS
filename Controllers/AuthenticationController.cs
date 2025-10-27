using Microsoft.AspNetCore.Mvc;
using CMS.Repositories.Interfaces;
using CMS.DTOs;
using CMS.Models;
using CMS.Helpers;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly IJwtServices _jwtService;

        public AuthenticationController(IUserAccount userAccount, IJwtServices jwtService)
        {
            _userAccount = userAccount;
            _jwtService = jwtService;
        }

        // Registration endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register user)
        {
            if (user == null)
                return BadRequest(new { message = "Model is empty" });

            try
            {
                var result = await _userAccount.CreateAsync(user);
                return Ok(new { message = "User registered successfully. Please verify OTP sent to email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // OTP Verification endpoint
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string email, [FromQuery] string otp)
        {
            var result = await _userAccount.VerifyOtpAsync(email, otp);
            if (!result)
                return BadRequest(new { message = "Invalid or expired OTP" });

            return Ok(new { message = "Email verified successfully" });
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto dto)
        {
            var sent = await _userAccount.ResendOtpAsync(dto.Email);
            if (!sent)
                return BadRequest(new { message = "User not found" });

            return Ok(new { message = "OTP resent successfully" });
        }



        // Login endpoint -> returns JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            if (user == null)
                return BadRequest(new { message = "Invalid request body" });

            var result = await _userAccount.SignInAsync(user);
            if (!result.Flag || result.User == null)
                return Unauthorized(new { message = result.Message ?? "Invalid credentials" });

            var token = _jwtService.GenerateToken(result.User);

            return Ok(new
            {
                message = "Login successful",
                token,
                role = result.User.Role
            });

        }

        // Change Password endpoint
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (model == null)
                return BadRequest(new { message = "Invalid request body" });

            var result = await _userAccount.ChangePasswordAsync(model);

            if (!result.Flag)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        // Forgot Password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var result = await _userAccount.ForgotPasswordAsync(model);
            if (!result.Flag)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }


        // Reset Password (via OTP or link)
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _userAccount.ResetPasswordAsync(model);
            if (!result.Flag)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }




    }
}

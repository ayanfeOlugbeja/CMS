

using CMS.DTOs;
using CMS.Entities;
using CMS.Responses;

namespace CMS.Repositories.Interfaces
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<bool> ResendOtpAsync(string email);
        Task<GeneralResponse> ChangePasswordAsync(ChangePasswordDto model);
        Task<GeneralResponse> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<GeneralResponse> ResetPasswordAsync(ResetPasswordDto model);



    }
}

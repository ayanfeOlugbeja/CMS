

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
    }
}

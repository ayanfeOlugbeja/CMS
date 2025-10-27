
namespace CMS.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public int? JobId { get; set; }
        // public Job? Job { get; set; }
        public string? PasswordResetOtp { get; set; }
        public string? PasswordResetToken { get; set; }


        public bool IsEmailVerified { get; set; } = false; 
        public string? EmailVerificationCode { get; set; } 
        public DateTime? OtpExpiryTime { get; set; }        // when OTP expires
        public DateTime? PasswordResetExpiry { get; set; }
    }
}

namespace CMS.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string OtpOrToken { get; set; }
        public string NewPassword { get; set; }
    }
}

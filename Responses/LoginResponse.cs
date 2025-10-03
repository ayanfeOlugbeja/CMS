

using CMS.Models;

namespace CMS.Responses
{
    public class LoginResponse
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public User? User { get; set; }
    }
}

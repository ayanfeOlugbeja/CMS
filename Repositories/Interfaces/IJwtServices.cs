using CMS.Models;
namespace CMS.Repositories.Interfaces
{
    public interface IJwtServices
    {
        string GenerateToken(User user);
    }
}


namespace CMS.Entities
{
    public class UserRole
    {

        public int Id { get; set; }  // Primary key

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int RoleId { get; set; }
        public SystemRole Role { get; set; }

    }
}




namespace CMS.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new();
    }
    
    public class RoleCreateUpdateDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
}


using System.Collections.Generic;

namespace CMS.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public int CompanyId { get; set; }
        public int branchId { get; set; }
        public int departmentId { get; set; }
        public int townId { get; set; }
        // public List<PermissionDto> PermissionIds { get; set; } = new();
        public int EmployeeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

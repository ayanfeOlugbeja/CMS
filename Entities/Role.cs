using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;

        public string? RoleDescription { get; set; }

        public int CompanyId { get; set; }

        // Relationship with permissions
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        public int EmployeeCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

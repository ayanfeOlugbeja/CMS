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

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}

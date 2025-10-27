using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }  

        public string ModuleName { get; set; } = string.Empty;
        public string? SubModuleName { get; set; }
        public string? Description { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}

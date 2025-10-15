namespace CMS.DTOs
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string ModuleName { get; set; }
        public string? SubModuleName { get; set; }
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public int branchId { get; set; }
        public int departmentId { get; set; }
        public int townId { get; set; }
    }
}

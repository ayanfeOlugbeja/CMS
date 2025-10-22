namespace CMS.Entities
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Relationships
        public ICollection<SubModule>? SubModules { get; set; }
        public ICollection<Permission>? Permissions { get; set; }
    }
}

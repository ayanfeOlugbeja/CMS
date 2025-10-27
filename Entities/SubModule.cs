namespace CMS.Entities
{
    public class SubModule
    {
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Foreign key
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
    }
}

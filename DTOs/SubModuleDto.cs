namespace CMS.DTOs
{
    public class SubModuleDto
    {
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int ModuleId { get; set; }
    }
}

namespace CMS.DTOs
{
    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Optional: include submodules for detailed responses
        public List<SubModuleDto>? SubModules { get; set; }
    }
}

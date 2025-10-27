

using System.ComponentModel.DataAnnotations;

namespace CMS.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department Name must be between 2 and 100 characters")]
        public string DepartmentName { get; set; } = string.Empty;
    }
    
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive number")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department Name must be between 2 and 100 characters")]
        public string DepartmentName { get; set; } = string.Empty;
    }
}


using System.ComponentModel.DataAnnotations;

namespace CMS.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobName { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public int? TelephoneNumber { get; set; }

        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public int TownId { get; set; }
        public string? TownName { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateEmployeeDto
    {
        [StringLength(20, ErrorMessage = "Civil ID cannot exceed 20 characters")]
        public string? CivilId { get; set; }
        
        [StringLength(20, ErrorMessage = "File Number cannot exceed 20 characters")]
        public string? FileNumber { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "Job Name cannot exceed 100 characters")]
        public string? JobName { get; set; }
        
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }
        
        [Required(ErrorMessage = "Telephone Number is required")]
        [Range(100000000, 999999999, ErrorMessage = "Telephone Number must be a valid 9-digit number")]
        public int TelephoneNumber { get; set; }
        
        [Required(ErrorMessage = "Department ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a positive number")]
        public int DepartmentId { get; set; }
        
        [Required(ErrorMessage = "Branch ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Branch ID must be a positive number")]
        public int BranchId { get; set; }
        
        [Required(ErrorMessage = "Town ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Town ID must be a positive number")]
        public int TownId { get; set; }
    }

    public class UpdateEmployeeDto
    {
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobName { get; set; }
        public string? Address { get; set; }
        public int? TelephoneNumber { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? TownId { get; set; }

    }
}


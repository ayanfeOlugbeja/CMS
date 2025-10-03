
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
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? JobName { get; set; }
        public string? Address { get; set; }
        public int TelephoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public int BranchId { get; set; }
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


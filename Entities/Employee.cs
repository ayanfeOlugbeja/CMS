
namespace CMS.Entities
{
    public class Employee : BaseEntity
    {
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? JobName { get; set; }
        public string? Address { get; set; }
        public int? TelephoneNumber { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public int TownId { get; set; }
        public Town? Town { get; set; }

    }
}

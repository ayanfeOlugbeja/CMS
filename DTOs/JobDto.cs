using System;

namespace CMS.DTOs.Job
{
    public class JobCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string EmploymentType { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? ClosingDate { get; set; }
    }

    public class JobUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string EmploymentType { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class JobReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string EmploymentType { get; set; }
        public decimal? Salary { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool IsActive { get; set; }
    }
}

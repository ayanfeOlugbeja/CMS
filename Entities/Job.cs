using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Department { get; set; }

        [Required]
        public string EmploymentType { get; set; } 

        public decimal? Salary { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ClosingDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

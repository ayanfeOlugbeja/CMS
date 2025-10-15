namespace CMS.Models
{

   public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal SalaryRange { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // public ICollection<ApplicationUser>? Users { get; set; }
}

}

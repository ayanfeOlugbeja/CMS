
namespace CMS.Entities
{
    public class Branch : BaseEntity
    {
        public string? BranchName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

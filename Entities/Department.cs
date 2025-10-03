
namespace CMS.Entities
{
    public class Department : BaseEntity
    {
        public string? DepartmentName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

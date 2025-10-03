
namespace CMS.Entities
{
    public class Town : BaseEntity
    {
        public string? TownName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

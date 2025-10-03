
using System.Text.Json.Serialization;

namespace CMS.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;
        //public string? Name { get; set; }

        ////Relationships : One to Many
        //[JsonIgnore]
        //public List<Employee>? Employees { get; set; }



    }
}

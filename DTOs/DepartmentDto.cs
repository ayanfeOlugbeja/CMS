

namespace CMS.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; }
    }
    public class UpdateDepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
    }
}

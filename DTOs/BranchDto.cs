


namespace CMS.DTOs
{
    public class BranchDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateBranchDto
    {
        public string BranchName { get; set; }
    }

    public class UpdateBranchDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
    }
}

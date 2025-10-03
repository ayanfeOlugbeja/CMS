
namespace CMS.DTOs
{
    public class TownDto
    {
        public int Id { get; set; }
        public string TownName { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTownDto
    {
        public string TownName { get; set; }
    }

    public class UpdateTownDto
    {
        public int Id { get; set; }
        public string TownName { get; set; }
    }
}

namespace WnT.Web.Models.DTO
{
    public class AddWalkDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double LengthInKM { get; set; }
        public string? WalkImageUrl { get; set; }

        public required RegionDTO Region { get; set; }
        public required DifficultyDTO Difficulty { get; set; }

        public List<RegionDTO> AvailableRegions { get; set; } = new List<RegionDTO>();
    }
}

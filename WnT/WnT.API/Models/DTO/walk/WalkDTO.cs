using WnT.API.Models.DTO.difficulty;
using WnT.API.Models.DTO.region;

namespace WnT.API.Models.DTO.walk
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double LengthInKM { get; set; }
        public string? WalkImageUrl { get; set; }

        public required RegionDTO Region { get; set; }
        public required DifficultyDTO Difficulty { get; set; }
    }
}

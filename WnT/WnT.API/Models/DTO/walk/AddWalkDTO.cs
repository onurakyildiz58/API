namespace WnT.API.Models.DTO.walk
{
    public class AddWalkDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double LengthInKM { get; set; }
        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}

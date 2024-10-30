namespace WnT.API.Models.DTO.region
{
    public class AddRegionDTO
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

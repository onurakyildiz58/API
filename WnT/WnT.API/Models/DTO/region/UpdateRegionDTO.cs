namespace WnT.API.Models.DTO.region
{
    public class UpdateRegionDTO
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

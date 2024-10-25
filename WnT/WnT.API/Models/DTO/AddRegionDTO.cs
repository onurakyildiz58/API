namespace WnT.API.Models.DTO
{
    public class AddRegionDTO
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

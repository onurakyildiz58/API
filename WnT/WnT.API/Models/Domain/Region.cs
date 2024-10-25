namespace WnT.API.Models.Domain
{
    public class Region : IRegion
    {
        public required Guid Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }

    public interface IRegion
    {
        Guid Id { get; }
        string Code { get; }
        string Name { get; }
        string? RegionImageUrl { get; }
    }
}

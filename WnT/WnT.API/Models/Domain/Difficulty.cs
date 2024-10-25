namespace WnT.API.Models.Domain
{
    public class Difficulty : IDifficulty
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }

    public interface IDifficulty
    {
        Guid Id { get; }
        string Name { get; }
    }
}

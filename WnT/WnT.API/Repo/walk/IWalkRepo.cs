using WnT.API.Models.Domain;

namespace WnT.API.Repo.walk
{
    public interface IWalkRepo
    {
        Task<List<Walk>> GetAllAsync(string? filterOn = null,
                                     string? filterQuery = null,
                                     string? sortBy = null,
                                     bool isAsc = true,
                                     int pageParam = 1,
                                     int pageSize = 10);
        Task<Walk?> GetByIdAsync(Guid Id);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid Id, Walk walk);
        Task<Walk?> DeleteAsync(Guid Id);
    }
}

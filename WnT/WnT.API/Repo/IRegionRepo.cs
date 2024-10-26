using WnT.API.Models.Domain;

namespace WnT.API.Repo
{
    public interface IRegionRepo
    {
       Task<List<Region>> GetAllAsync();
       Task<Region?> GetByIdAsync(Guid Id);
       Task<Region> CreateAsync(Region region);
       Task<Region?> UpdateAsync(Guid Id, Region region);
       Task<Region?> DeleteAsync(Guid Id);
    }
}

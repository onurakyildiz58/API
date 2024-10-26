using Microsoft.EntityFrameworkCore;
using WnT.API.Data;
using WnT.API.Models.Domain;
using WnT.API.Models.DTO;
namespace WnT.API.Repo
{
    public class SQLRegionRepo : IRegionRepo
    {
        private readonly WnTDbContext dbContext;

        public SQLRegionRepo(WnTDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid Id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid Id, Region region)
        {
            var regionExists = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (regionExists is null)
            {
                return null;
            }

            regionExists.Code = region.Code;
            regionExists.Name = region.Name;
            regionExists.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid Id)
        {
            var regionExists = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (regionExists is null)
            {
                return null;
            }

            dbContext.Regions.Remove(regionExists);
            await dbContext.SaveChangesAsync();

            return regionExists;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WnT.API.Data;
using WnT.API.Models.Domain;

namespace WnT.API.Repo.walk
{
    public class SQLWalkRepo : IWalkRepo
    {
        private readonly WnTDbContext dbContext;

        public SQLWalkRepo(WnTDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
                                                            
        public async Task<List<Walk>> GetAllAsync()
        {
            return await dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid Id)
        {
            return await dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid Id, Walk walk)
        {
            var walkExists = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == Id);

            if (walkExists is null)
            {
                return null;
            }

            walkExists.Name = walk.Name;
            walkExists.Description = walk.Description;
            walkExists.LengthInKM = walk.LengthInKM;
            walkExists.WalkImageUrl = walk.WalkImageUrl;
            walkExists.DifficultyId = walk.DifficultyId;
            walkExists.RegionId = walk.RegionId;
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid Id)
        {
            var walkExists = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == Id);

            if (walkExists is null)
            {
                return null;
            }

            dbContext.Walks.Remove(walkExists);
            await dbContext.SaveChangesAsync();

            return walkExists;
        }
    }
}

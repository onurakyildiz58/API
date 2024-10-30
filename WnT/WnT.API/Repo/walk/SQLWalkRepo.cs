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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null,
                                                  string? filterQuery = null,
                                                  string? sortBy = null,
                                                  bool isAsc = true,
                                                  int pageParam = 1,
                                                  int pageSize = 10)
        {
            var walks = dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAsc ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAsc ? walks.OrderBy(x => x.LengthInKM) : walks.OrderByDescending(x => x.LengthInKM);
                }
            }


            //pagination
            var pagination = (pageParam - 1) * pageSize;
            // if user want to get page 2 calculation says (2-1)*10 = 10 so its gonna skip first 10 result and take 10 result after them
            // skip pagination take pageSize basicly

            return await walks.Skip(pagination).Take(pageSize).ToListAsync();
            //return await dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).ToListAsync();
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

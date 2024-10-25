using Microsoft.EntityFrameworkCore;
using WnT.API.Models.Domain;

namespace WnT.API.Data
{
    public class WnTDbContext : DbContext
    {
        public WnTDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}

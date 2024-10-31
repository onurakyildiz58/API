using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WnT.API.Models.Domain;

namespace WnT.API.Data
{
    public class WnTDbAuthContext : IdentityDbContext

    {
        public WnTDbAuthContext(DbContextOptions<WnTDbAuthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var readerRoleId = "4382af7d-da73-4cbe-a038-442eb85d5a9d";
            var writerRoleId = "390754dd-5835-44e1-851b-ec9b7f096cc7";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}

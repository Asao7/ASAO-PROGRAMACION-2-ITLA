using Microsoft.EntityFrameworkCore;
using Pets.API.Models;

namespace Pets.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Pets.Domain.Entities;

namespace Pets.Infrastructure.Context
{
    public class PetsDbContext : DbContext
    {
        public PetsDbContext(DbContextOptions<PetsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
    }
}
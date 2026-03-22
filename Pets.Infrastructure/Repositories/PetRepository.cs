using Microsoft.EntityFrameworkCore;
using Pets.Domain.Entities;
using Pets.Domain.Repository;
using Pets.Infrastructure.Context;
using Pets.Infrastructure.Core;

namespace Pets.Infrastructure.Repositories
{
    public class PetRepository : BaseRepository<Pet>, IPetRepository
    {
        public PetRepository(PetsDbContext context) : base(context)
        {
        }

        public async Task<List<Pet>> GetAllAsync()
        {
            return await _context.Pets.ToListAsync();
        }

        public async Task<Pet?> GetByIdAsync(int id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task AddAsync(Pet pet)
        {
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pet pet)
        {
            _context.Pets.Update(pet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();
            }
        }
    }
}
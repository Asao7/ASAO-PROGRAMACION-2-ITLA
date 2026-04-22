using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;

namespace ExcursionManager.Application.Services
{
    public class GuideService : IGuideService
    {
        private readonly IRepository<Guide> _repository;

        public GuideService(IRepository<Guide> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GuideDto>> GetAllAsync()
        {
            var guides = await _repository.GetAllAsync();
            return guides.Select(MapToDto);
        }

        public async Task<IEnumerable<GuideDto>> GetActiveAsync()
        {
            var guides = await _repository.GetAllAsync();
            return guides.Where(g => g.IsActive).Select(MapToDto);
        }

        public async Task<GuideDto?> GetByIdAsync(int id)
        {
            var guide = await _repository.GetByIdAsync(id);
            return guide == null ? null : MapToDto(guide);
        }

        // Overloaded search by name
        public async Task<IEnumerable<GuideDto>> SearchAsync(string name)
        {
            var guides = await _repository.GetAllAsync();
            return guides
                .Where(g => g.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        // Overloaded search by specialty
        public async Task<IEnumerable<GuideDto>> SearchBySpecialtyAsync(string specialty)
        {
            var guides = await _repository.GetAllAsync();
            return guides
                .Where(g => g.Specialty.Contains(specialty, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        public async Task<int> CreateAsync(CreateGuideDto dto)
        {
            var guide = new Guide(dto.FullName, dto.IdNumber,
                dto.Specialty, dto.Phone, dto.Email);
            return await _repository.CreateAsync(guide);
        }

        public async Task<bool> UpdateAsync(int id, CreateGuideDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;
            var updated = new Guide(id, dto.FullName, dto.IdNumber,
                dto.Specialty, dto.Phone, dto.Email,
                existing.IsActive, existing.CreatedAt);
            return await _repository.UpdateAsync(updated);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);

        private static GuideDto MapToDto(Guide g) => new()
        {
            Id = g.Id,
            FullName = g.FullName,
            IdNumber = g.IdNumber,
            Specialty = g.Specialty,
            Phone = g.Phone,
            Email = g.Email,
            IsActive = g.IsActive
        };
    }
}
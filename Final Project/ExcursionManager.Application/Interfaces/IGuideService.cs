using ExcursionManager.Application.DTOs;

namespace ExcursionManager.Application.Interfaces
{
    public interface IGuideService
    {
        Task<IEnumerable<GuideDto>> GetAllAsync();
        Task<IEnumerable<GuideDto>> GetActiveAsync();
        Task<GuideDto?> GetByIdAsync(int id);
        // Overloaded search - by name or by specialty
        Task<IEnumerable<GuideDto>> SearchAsync(string name);
        Task<IEnumerable<GuideDto>> SearchBySpecialtyAsync(string specialty);
        Task<int> CreateAsync(CreateGuideDto dto);
        Task<bool> UpdateAsync(int id, CreateGuideDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
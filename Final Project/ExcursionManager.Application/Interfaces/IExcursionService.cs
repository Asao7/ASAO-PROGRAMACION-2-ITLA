using ExcursionManager.Application.DTOs;

namespace ExcursionManager.Application.Interfaces
{
    public interface IExcursionService
    {
        Task<IEnumerable<ExcursionDto>> GetAllAsync();
        Task<IEnumerable<ExcursionDto>> GetAvailableAsync();
        Task<ExcursionDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateExcursionDto dto);
        Task<bool> UpdateAsync(int id, UpdateExcursionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> CancelAsync(int id);
    }
}
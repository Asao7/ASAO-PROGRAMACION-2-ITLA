using ExcursionManager.Application.DTOs;

namespace ExcursionManager.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDto>> GetAllAsync();
        Task<ParticipantDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateParticipantDto dto);
        Task<bool> UpdateAsync(int id, CreateParticipantDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
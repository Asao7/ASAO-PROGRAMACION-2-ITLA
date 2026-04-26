using ExcursionManager.Application.DTOs;

namespace ExcursionManager.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<IEnumerable<ReservationDto>> GetByExcursionAsync(int excursionId);
        Task<ReservationDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateReservationDto dto);
        Task<bool> ConfirmAsync(int id);
        Task<bool> CancelAsync(int id);
        Task<bool> MarkAttendanceAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
using ExcursionManager.Application.DTOs;

namespace ExcursionManager.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<int> CreateUserAsync(CreateUserDto dto);
        Task<IEnumerable<AuthResponseDto>> GetAllUsersAsync();
    }
}
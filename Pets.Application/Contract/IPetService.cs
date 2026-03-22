using Pets.Application.Core;
using Pets.Application.Dtos.Pet;

namespace Pets.Application.Contract
{
    public interface IPetService
    {
        Task<ServiceResult<List<PetDto>>> GetAllAsync();
        Task<ServiceResult<PetDto>> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(CreatePetDto dto);
        Task<ServiceResult> UpdateAsync(int id, UpdatePetDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}

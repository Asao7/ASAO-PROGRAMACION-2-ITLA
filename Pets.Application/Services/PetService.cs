using Pets.Application.Contract;
using Pets.Application.Core;
using Pets.Application.Dtos.Pet;
using Pets.Domain.Entities;
using Pets.Domain.Repository;

namespace Pets.Application.Services
{
    public class PetService : BaseService, IPetService
    {
        private readonly IPetRepository _repository;

        public PetService(IPetRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<PetDto>>> GetAllAsync()
        {
            var pets = await _repository.GetAllAsync();
            return new ServiceResult<List<PetDto>>
            {
                Success = true,
                Data = pets.Select(p => new PetDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Age = p.Age,
                    Species = p.Species
                }).ToList()
            };
        }

        public async Task<ServiceResult<PetDto>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return new ServiceResult<PetDto> { Success = false, Message = "El ID debe ser mayor a 0." };

            var pet = await _repository.GetByIdAsync(id);
            if (pet == null)
                return new ServiceResult<PetDto> { Success = false, Message = "Mascota no encontrada." };

            return new ServiceResult<PetDto>
            {
                Success = true,
                Data = new PetDto { Id = pet.Id, Name = pet.Name, Age = pet.Age, Species = pet.Species }
            };
        }

        public async Task<ServiceResult> CreateAsync(CreatePetDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return new ServiceResult { Success = false, Message = "El nombre es requerido." };
            if (dto.Name.Length > 50)
                return new ServiceResult { Success = false, Message = "El nombre no puede exceder 50 caracteres." };
            if (string.IsNullOrWhiteSpace(dto.Species))
                return new ServiceResult { Success = false, Message = "La especie es requerida." };
            if (dto.Species.Length > 50)
                return new ServiceResult { Success = false, Message = "La especie no puede exceder 50 caracteres." };
            if (dto.Age <= 0)
                return new ServiceResult { Success = false, Message = "La edad debe ser mayor a 0." };
            if (dto.Age > 50)
                return new ServiceResult { Success = false, Message = "La edad no es valida." };

            var pet = new Pet { Name = dto.Name, Age = dto.Age, Species = dto.Species };
            await _repository.AddAsync(pet);
            return new ServiceResult { Success = true, Message = "Mascota creada exitosamente." };
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdatePetDto dto)
        {
            if (id <= 0)
                return new ServiceResult { Success = false, Message = "El ID no es valido." };
            if (string.IsNullOrWhiteSpace(dto.Name))
                return new ServiceResult { Success = false, Message = "El nombre es requerido." };
            if (dto.Name.Length > 50)
                return new ServiceResult { Success = false, Message = "El nombre no puede exceder 50 caracteres." };
            if (string.IsNullOrWhiteSpace(dto.Species))
                return new ServiceResult { Success = false, Message = "La especie es requerida." };
            if (dto.Species.Length > 50)
                return new ServiceResult { Success = false, Message = "La especie no puede exceder 50 caracteres." };
            if (dto.Age <= 0)
                return new ServiceResult { Success = false, Message = "La edad debe ser mayor a 0." };
            if (dto.Age > 50)
                return new ServiceResult { Success = false, Message = "La edad no es valida." };

            var pet = await _repository.GetByIdAsync(id);
            if (pet == null)
                return new ServiceResult { Success = false, Message = "Mascota no encontrada." };

            pet.Name = dto.Name;
            pet.Age = dto.Age;
            pet.Species = dto.Species;
            await _repository.UpdateAsync(pet);
            return new ServiceResult { Success = true, Message = "Mascota actualizada exitosamente." };
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return new ServiceResult { Success = false, Message = "El ID no es valido." };

            var pet = await _repository.GetByIdAsync(id);
            if (pet == null)
                return new ServiceResult { Success = false, Message = "Mascota no encontrada." };

            await _repository.DeleteAsync(id);
            return new ServiceResult { Success = true, Message = "Mascota eliminada exitosamente." };
        }
    }
}

using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;

namespace ExcursionManager.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRepository<Participant> _repository;

        public ParticipantService(IRepository<Participant> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ParticipantDto>> GetAllAsync()
        {
            var participants = await _repository.GetAllAsync();
            return participants.Select(MapToDto);
        }

        public async Task<ParticipantDto?> GetByIdAsync(int id)
        {
            var participant = await _repository.GetByIdAsync(id);
            return participant == null ? null : MapToDto(participant);
        }

        public async Task<int> CreateAsync(CreateParticipantDto dto)
        {
            // Validate age
            if (dto.Age < 5 || dto.Age > 100)
                throw new ArgumentException("Age must be between 5 and 100.");

            var participant = new Participant(
                dto.FullName, dto.IdNumber, dto.Age,
                dto.EmergencyContact, dto.Email);

            return await _repository.CreateAsync(participant);
        }

        public async Task<bool> UpdateAsync(int id, CreateParticipantDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            var updated = new Participant(id, dto.FullName, dto.IdNumber,
                dto.Age, dto.EmergencyContact, dto.Email, existing.CreatedAt);

            return await _repository.UpdateAsync(updated);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);

        private static ParticipantDto MapToDto(Participant p) => new()
        {
            Id = p.Id,
            FullName = p.FullName,
            IdNumber = p.IdNumber,
            Age = p.Age,
            EmergencyContact = p.EmergencyContact,
            Email = p.Email
        };
    }
}
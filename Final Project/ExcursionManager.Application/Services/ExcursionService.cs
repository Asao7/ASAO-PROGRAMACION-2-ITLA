using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;

namespace ExcursionManager.Application.Services
{
    public class ExcursionService : IExcursionService
    {
        private readonly IRepository<Excursion> _excursionRepository;
        private readonly IRepository<Guide> _guideRepository;
        private readonly IRepository<Domain.Entities.Route> _routeRepository;

        public ExcursionService(
            IRepository<Excursion> excursionRepository,
            IRepository<Guide> guideRepository,
            IRepository<Domain.Entities.Route> routeRepository)
        {
            _excursionRepository = excursionRepository;
            _guideRepository = guideRepository;
            _routeRepository = routeRepository;
        }

        public async Task<IEnumerable<ExcursionDto>> GetAllAsync()
        {
            var excursions = await _excursionRepository.GetAllAsync();
            var dtos = new List<ExcursionDto>();
            foreach (var e in excursions)
            {
                var dto = MapToDto(e);
                var route = await _routeRepository.GetByIdAsync(e.RouteId);
                if (route != null) dto.RouteName = route.Name;
                if (e.GuideId.HasValue)
                {
                    var guide = await _guideRepository.GetByIdAsync(e.GuideId.Value);
                    if (guide != null) dto.GuideName = guide.FullName;
                }
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<ExcursionDto>> GetAvailableAsync()
        {
            var all = await GetAllAsync();
            return all.Where(e => e.Status == "Active" && e.AvailableSpots > 0);
        }

        public async Task<ExcursionDto?> GetByIdAsync(int id)
        {
            var excursion = await _excursionRepository.GetByIdAsync(id);
            if (excursion == null) return null;
            var dto = MapToDto(excursion);
            var route = await _routeRepository.GetByIdAsync(excursion.RouteId);
            if (route != null) dto.RouteName = route.Name;
            if (excursion.GuideId.HasValue)
            {
                var guide = await _guideRepository.GetByIdAsync(excursion.GuideId.Value);
                if (guide != null) dto.GuideName = guide.FullName;
            }
            return dto;
        }

        public async Task<int> CreateAsync(CreateExcursionDto dto)
        {
            // Validate departure date is in the future
            if (dto.DepartureDate <= DateTime.Now)
                throw new ArgumentException("Departure date must be in the future.");

            // Validate capacity
            if (dto.MaxCapacity <= 0)
                throw new ArgumentException("Max capacity must be greater than zero.");

            var excursion = new Excursion(
                dto.Name, dto.Description, dto.RouteId,
                dto.GuideId ?? 0, dto.DepartureDate,
                dto.MaxCapacity, dto.Price);

            return await _excursionRepository.CreateAsync(excursion);
        }

        public async Task<bool> UpdateAsync(int id, UpdateExcursionDto dto)
        {
            var excursion = await _excursionRepository.GetByIdAsync(id);
            if (excursion == null) return false;

            var updated = new Excursion(
                id, dto.Name, dto.Description, excursion.RouteId,
                dto.GuideId, dto.DepartureDate, excursion.MaxCapacity,
                excursion.AvailableSpots, dto.Price,
                excursion.Status, excursion.CreatedAt);

            return await _excursionRepository.UpdateAsync(updated);
        }

        public async Task<bool> CancelAsync(int id)
        {
            var excursion = await _excursionRepository.GetByIdAsync(id);
            if (excursion == null) return false;
            excursion.Cancel();
            return await _excursionRepository.UpdateAsync(excursion);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _excursionRepository.DeleteAsync(id);

        // Private mapper
        private static ExcursionDto MapToDto(Excursion e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            RouteId = e.RouteId,
            GuideId = e.GuideId,
            DepartureDate = e.DepartureDate,
            MaxCapacity = e.MaxCapacity,
            AvailableSpots = e.AvailableSpots,
            Price = e.Price,
            Status = e.Status
        };
    }
}
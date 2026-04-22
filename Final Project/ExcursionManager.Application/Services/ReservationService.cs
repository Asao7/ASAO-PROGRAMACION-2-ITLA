using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;

namespace ExcursionManager.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Excursion> _excursionRepository;
        private readonly IRepository<Participant> _participantRepository;

        public ReservationService(
            IRepository<Reservation> reservationRepository,
            IRepository<Excursion> excursionRepository,
            IRepository<Participant> participantRepository)
        {
            _reservationRepository = reservationRepository;
            _excursionRepository = excursionRepository;
            _participantRepository = participantRepository;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return await EnrichReservations(reservations);
        }

        public async Task<IEnumerable<ReservationDto>> GetByExcursionAsync(int excursionId)
        {
            var all = await _reservationRepository.GetAllAsync();
            var filtered = all.Where(r => r.ExcursionId == excursionId);
            return await EnrichReservations(filtered);
        }

        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return null;
            var enriched = await EnrichReservations(new[] { reservation });
            return enriched.FirstOrDefault();
        }

        public async Task<int> CreateAsync(CreateReservationDto dto)
        {
            // Validate excursion exists and has spots
            var excursion = await _excursionRepository.GetByIdAsync(dto.ExcursionId);
            if (excursion == null)
                throw new ArgumentException("Excursion not found.");
            if (!excursion.IsAvailable())
                throw new InvalidOperationException("Excursion is not available or full.");

            // Validate participant exists
            var participant = await _participantRepository.GetByIdAsync(dto.ParticipantId);
            if (participant == null)
                throw new ArgumentException("Participant not found.");

            // Book the spot and update excursion
            excursion.BookSpot();
            await _excursionRepository.UpdateAsync(excursion);

            var reservation = new Reservation(dto.ParticipantId, dto.ExcursionId);
            reservation.Confirm();
            return await _reservationRepository.CreateAsync(reservation);
        }

        public async Task<bool> ConfirmAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return false;
            reservation.Confirm();
            return await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task<bool> CancelAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return false;

            // Release the spot if it was confirmed
            if (reservation.Status == "Confirmed")
            {
                var excursion = await _excursionRepository.GetByIdAsync(reservation.ExcursionId);
                if (excursion != null)
                {
                    excursion.ReleaseSpot();
                    await _excursionRepository.UpdateAsync(excursion);
                }
            }

            reservation.Cancel();
            return await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task<bool> MarkAttendanceAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return false;
            reservation.MarkAttendance();
            return await _reservationRepository.UpdateAsync(reservation);
        }

        // Enrich reservations with participant and excursion names
        private async Task<IEnumerable<ReservationDto>> EnrichReservations(
            IEnumerable<Reservation> reservations)
        {
            var dtos = new List<ReservationDto>();
            foreach (var r in reservations)
            {
                var participant = await _participantRepository.GetByIdAsync(r.ParticipantId);
                var excursion = await _excursionRepository.GetByIdAsync(r.ExcursionId);
                dtos.Add(new ReservationDto
                {
                    Id = r.Id,
                    ParticipantId = r.ParticipantId,
                    ParticipantName = participant?.FullName ?? "Unknown",
                    ExcursionId = r.ExcursionId,
                    ExcursionName = excursion?.Name ?? "Unknown",
                    ReservedAt = r.ReservedAt,
                    Status = r.Status,
                    Attended = r.Attended
                });
            }
            return dtos;
        }
    }
}

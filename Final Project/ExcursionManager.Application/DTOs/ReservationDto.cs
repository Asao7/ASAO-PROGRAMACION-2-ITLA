namespace ExcursionManager.Application.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public int ExcursionId { get; set; }
        public string ExcursionName { get; set; } = string.Empty;
        public DateTime ReservedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool Attended { get; set; }
    }

    public class CreateReservationDto
    {
        public int ParticipantId { get; set; }
        public int ExcursionId { get; set; }
    }
}

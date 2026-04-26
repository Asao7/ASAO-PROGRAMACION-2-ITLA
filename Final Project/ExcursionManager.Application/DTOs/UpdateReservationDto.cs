namespace ExcursionManager.Application.DTOs
{
    public class UpdateReservationDto
    {
        public int ParticipantId { get; set; }
        public int ExcursionId { get; set; }
        public string? Status { get; set; }
        public bool Attended { get; set; }
    }
}
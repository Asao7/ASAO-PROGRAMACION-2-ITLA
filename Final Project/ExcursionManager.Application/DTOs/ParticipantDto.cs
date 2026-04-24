namespace ExcursionManager.Application.DTOs
{
    public class ParticipantDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public int Age { get; set; }
        public string EmergencyContact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CreateParticipantDto
    {
        public string FullName { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public int Age { get; set; }
        public string EmergencyContact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
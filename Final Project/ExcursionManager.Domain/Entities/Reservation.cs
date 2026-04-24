namespace ExcursionManager.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public int ParticipantId { get; private set; }
        public int ExcursionId { get; private set; }
        public DateTime ReservedAt { get; private set; }
        public string Status { get; private set; } // Pending, Confirmed, Cancelled
        public bool Attended { get; private set; }

        // Navigation properties
        public Participant? Participant { get; set; }
        public Excursion? Excursion { get; set; }

        // Basic constructor
        public Reservation(int participantId, int excursionId)
        {
            ParticipantId = participantId;
            ExcursionId = excursionId;
            ReservedAt = DateTime.Now;
            Status = "Pending";
            Attended = false;
        }

        // Overloaded full constructor (from database)
        public Reservation(int id, int participantId, int excursionId,
                           DateTime reservedAt, string status, bool attended, DateTime createdAt)
            : base(id, createdAt)
        {
            ParticipantId = participantId;
            ExcursionId = excursionId;
            ReservedAt = reservedAt;
            Status = status;
            Attended = attended;
        }

        public void Confirm() { if (Status == "Pending") Status = "Confirmed"; }
        public void Cancel() { Status = "Cancelled"; }
        public void MarkAttendance() { if (Status == "Confirmed") Attended = true; }

        public override string ToString() =>
            $"[Reservation #{Id}] Participant: {ParticipantId} | " +
            $"Excursion: {ExcursionId} | Status: {Status} | Attended: {Attended}";
    }
}
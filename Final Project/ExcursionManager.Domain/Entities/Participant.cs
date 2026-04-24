namespace ExcursionManager.Domain.Entities
{
    public class Participant : BaseEntity
    {
        public string FullName { get; private set; }
        public string IdNumber { get; private set; }
        public int Age { get; private set; }
        public string EmergencyContact { get; set; }
        public string Email { get; set; }

        // Basic constructor
        public Participant(string fullName, string idNumber, int age)
        {
            FullName = fullName;
            IdNumber = idNumber;
            Age = age;
            EmergencyContact = string.Empty;
            Email = string.Empty;
        }

        // Overloaded full constructor
        public Participant(string fullName, string idNumber, int age,
                           string emergencyContact, string email)
            : this(fullName, idNumber, age)
        {
            EmergencyContact = emergencyContact;
            Email = email;
        }

        // Overloaded constructor from database
        public Participant(int id, string fullName, string idNumber, int age,
                           string emergencyContact, string email, DateTime createdAt)
            : base(id, createdAt)
        {
            FullName = fullName;
            IdNumber = idNumber;
            Age = age;
            EmergencyContact = emergencyContact;
            Email = email;
        }

        public override string ToString() =>
            $"[Participant #{Id}] {FullName} | ID: {IdNumber} | Age: {Age}";
    }
}
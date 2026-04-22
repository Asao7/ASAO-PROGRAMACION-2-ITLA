namespace ExcursionManager.Domain.Entities
{
    public class Guide : BaseEntity
    {
        public string FullName { get; private set; }
        public string IdNumber { get; private set; }
        public string Specialty { get; private set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; private set; }

        // Basic constructor
        public Guide(string fullName, string idNumber, string specialty)
        {
            FullName = fullName;
            IdNumber = idNumber;
            Specialty = specialty;
            IsActive = true;
            Phone = string.Empty;
            Email = string.Empty;
        }

        // Overloaded constructor with contact info
        public Guide(string fullName, string idNumber, string specialty,
                     string phone, string email)
            : this(fullName, idNumber, specialty)
        {
            Phone = phone;
            Email = email;
        }

        // Overloaded full constructor (from database)
        public Guide(int id, string fullName, string idNumber, string specialty,
                     string phone, string email, bool isActive, DateTime createdAt)
            : base(id, createdAt)
        {
            FullName = fullName;
            IdNumber = idNumber;
            Specialty = specialty;
            Phone = phone;
            Email = email;
            IsActive = isActive;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        public override string ToString() =>
            $"[Guide #{Id}] {FullName} | Specialty: {Specialty} | Phone: {Phone}";
    }
}
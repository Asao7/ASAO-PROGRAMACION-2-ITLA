namespace ExcursionManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; } // Admin, Guide, Staff
        public bool IsActive { get; private set; }

        // Basic constructor
        public User(string username, string passwordHash, string fullName, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            FullName = fullName;
            Role = role;
            Email = string.Empty;
            IsActive = true;
        }

        // Overloaded full constructor (from database)
        public User(int id, string username, string passwordHash, string fullName,
                    string email, string role, bool isActive, DateTime createdAt)
            : base(id, createdAt)
        {
            Username = username;
            PasswordHash = passwordHash;
            FullName = fullName;
            Email = email;
            Role = role;
            IsActive = isActive;
        }

        public override string ToString() =>
            $"[User #{Id}] {FullName} | Role: {Role} | Active: {IsActive}";
    }
}
namespace ExcursionManager.Domain.Entities
{
    // Abstract base class - all entities inherit from this
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        // Base constructor
        protected BaseEntity()
        {
            CreatedAt = DateTime.Now;
        }

        // Overloaded constructor with existing id
        protected BaseEntity(int id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
        }

        // Overloaded constructor with id and date
        protected BaseEntity(int id, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
        }

        // Abstract method - every entity must implement its own string representation
        public abstract override string ToString();
    }
}
using Pets.Domain.Core;

namespace Pets.Domain.Entities
{
    public class Pet : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public int Age { get; set; }

        public string? OwnerName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
namespace Pets.API.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public string? OwnerName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

namespace ExcursionManager.Domain.Entities
{
    public class Route : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal DistanceKm { get; private set; }
        public string Difficulty { get; private set; } // Easy, Moderate, Hard
        public string StartPoint { get; private set; }
        public string EndPoint { get; private set; }

        // Basic constructor
        public Route(string name, string difficulty, string startPoint, string endPoint)
        {
            Name = name;
            Difficulty = difficulty;
            StartPoint = startPoint;
            EndPoint = endPoint;
            Description = string.Empty;
        }

        // Overloaded full constructor
        public Route(int id, string name, string description, decimal distanceKm,
                     string difficulty, string startPoint, string endPoint, DateTime createdAt)
            : base(id, createdAt)
        {
            Name = name;
            Description = description;
            DistanceKm = distanceKm;
            Difficulty = difficulty;
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override string ToString() =>
            $"[Route #{Id}] {Name} | {Difficulty} | {DistanceKm}km | {StartPoint} → {EndPoint}";
    }
}
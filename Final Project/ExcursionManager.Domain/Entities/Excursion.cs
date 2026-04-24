namespace ExcursionManager.Domain.Entities
{
    public class Excursion : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int RouteId { get; private set; }
        public int? GuideId { get; private set; }
        public DateTime DepartureDate { get; private set; }
        public int MaxCapacity { get; private set; }
        public int AvailableSpots { get; private set; }
        public decimal Price { get; private set; }
        public string Status { get; private set; } // Active, Full, Cancelled

        // Navigation properties
        public Route? Route { get; set; }
        public Guide? Guide { get; set; }

        // Basic constructor
        public Excursion(string name, int routeId, DateTime departureDate,
                         int maxCapacity, decimal price)
        {
            Name = name;
            RouteId = routeId;
            DepartureDate = departureDate;
            MaxCapacity = maxCapacity;
            AvailableSpots = maxCapacity;
            Price = price;
            Status = "Active";
            Description = string.Empty;
        }

        // Overloaded constructor with guide
        public Excursion(string name, string description, int routeId, int guideId,
                         DateTime departureDate, int maxCapacity, decimal price)
            : this(name, routeId, departureDate, maxCapacity, price)
        {
            Description = description;
            GuideId = guideId;
        }

        // Overloaded full constructor (from database)
        public Excursion(int id, string name, string description, int routeId,
                         int? guideId, DateTime departureDate, int maxCapacity,
                         int availableSpots, decimal price, string status, DateTime createdAt)
            : base(id, createdAt)
        {
            Name = name;
            Description = description;
            RouteId = routeId;
            GuideId = guideId;
            DepartureDate = departureDate;
            MaxCapacity = maxCapacity;
            AvailableSpots = availableSpots;
            Price = price;
            Status = status;
        }

        public bool IsAvailable() => Status == "Active" && AvailableSpots > 0;

        public bool BookSpot()
        {
            if (!IsAvailable()) return false;
            AvailableSpots--;
            if (AvailableSpots == 0) Status = "Full";
            return true;
        }

        public void ReleaseSpot()
        {
            if (AvailableSpots < MaxCapacity)
            {
                AvailableSpots++;
                if (Status == "Full") Status = "Active";
            }
        }

        public void Cancel() => Status = "Cancelled";

        public override string ToString() =>
            $"[Excursion #{Id}] {Name} | {DepartureDate:dd/MM/yyyy} | " +
            $"Spots: {AvailableSpots}/{MaxCapacity} | ${Price} | Status: {Status}";
    }
}
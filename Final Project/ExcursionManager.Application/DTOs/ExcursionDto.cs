namespace ExcursionManager.Application.DTOs
{
    // Data Transfer Objects - what the API sends/receives
    public class ExcursionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RouteId { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int? GuideId { get; set; }
        public string GuideName { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public int MaxCapacity { get; set; }
        public int AvailableSpots { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateExcursionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RouteId { get; set; }
        public int? GuideId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int MaxCapacity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateExcursionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? GuideId { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }
    }
}

using Dapper;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;

namespace ExcursionManager.Persistence.Repositories
{
    public class ExcursionRepository : IRepository<Excursion>
    {
        private readonly DatabaseContext _context;

        public ExcursionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Excursion>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT excursion_id AS Id, name AS Name, description AS Description,
                               route_id AS RouteId, guide_id AS GuideId,
                               departure_date AS DepartureDate, max_capacity AS MaxCapacity,
                               available_spots AS AvailableSpots, price AS Price,
                               status AS Status, created_at AS CreatedAt
                        FROM Excursions ORDER BY departure_date";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(e => new Excursion(
                (int)e.Id, (string)e.Name, (string)(e.Description ?? ""),
                (int)e.RouteId, (int?)e.GuideId, (DateTime)e.DepartureDate,
                (int)e.MaxCapacity, (int)e.AvailableSpots,
                (decimal)e.Price, (string)e.Status, (DateTime)e.CreatedAt));
        }

        public async Task<Excursion?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT excursion_id AS Id, name AS Name, description AS Description,
                               route_id AS RouteId, guide_id AS GuideId,
                               departure_date AS DepartureDate, max_capacity AS MaxCapacity,
                               available_spots AS AvailableSpots, price AS Price,
                               status AS Status, created_at AS CreatedAt
                        FROM Excursions WHERE excursion_id = @Id";
            var e = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            if (e == null) return null;
            return new Excursion((int)e.Id, (string)e.Name, (string)(e.Description ?? ""),
                (int)e.RouteId, (int?)e.GuideId, (DateTime)e.DepartureDate,
                (int)e.MaxCapacity, (int)e.AvailableSpots,
                (decimal)e.Price, (string)e.Status, (DateTime)e.CreatedAt);
        }

        public async Task<IEnumerable<Excursion>> GetAvailableAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT excursion_id AS Id, name AS Name, description AS Description,
                               route_id AS RouteId, guide_id AS GuideId,
                               departure_date AS DepartureDate, max_capacity AS MaxCapacity,
                               available_spots AS AvailableSpots, price AS Price,
                               status AS Status, created_at AS CreatedAt
                        FROM Excursions 
                        WHERE status = 'Active' AND available_spots > 0
                        ORDER BY departure_date";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(e => new Excursion(
                (int)e.Id, (string)e.Name, (string)(e.Description ?? ""),
                (int)e.RouteId, (int?)e.GuideId, (DateTime)e.DepartureDate,
                (int)e.MaxCapacity, (int)e.AvailableSpots,
                (decimal)e.Price, (string)e.Status, (DateTime)e.CreatedAt));
        }

        public async Task<int> CreateAsync(Excursion entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Excursions 
                            (name, description, route_id, guide_id, departure_date,
                             max_capacity, available_spots, price)
                        OUTPUT INSERTED.excursion_id
                        VALUES (@Name, @Description, @RouteId, @GuideId, @DepartureDate,
                                @MaxCapacity, @MaxCapacity, @Price)";
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                entity.Name,
                entity.Description,
                entity.RouteId,
                entity.GuideId,
                entity.DepartureDate,
                entity.MaxCapacity,
                entity.Price
            });
        }

        public async Task<bool> UpdateAsync(Excursion entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Excursions SET name = @Name, description = @Description,
                               guide_id = @GuideId, departure_date = @DepartureDate,
                               available_spots = @AvailableSpots, price = @Price,
                               status = @Status
                        WHERE excursion_id = @Id";
            var rows = await connection.ExecuteAsync(sql, new
            {
                entity.Name,
                entity.Description,
                entity.GuideId,
                entity.DepartureDate,
                entity.AvailableSpots,
                entity.Price,
                entity.Status,
                entity.Id
            });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(
                "DELETE FROM Excursions WHERE excursion_id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}
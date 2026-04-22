using Dapper;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;

namespace ExcursionManager.Persistence.Repositories
{
    public class RouteRepository : IRepository<Route>
    {
        private readonly DatabaseContext _context;

        public RouteRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Route>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT route_id AS Id, name AS Name, description AS Description,
                               distance_km AS DistanceKm, difficulty AS Difficulty,
                               start_point AS StartPoint, end_point AS EndPoint,
                               created_at AS CreatedAt
                        FROM Routes ORDER BY name";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(r => new Route(
                (int)r.Id, (string)r.Name, (string)(r.Description ?? ""),
                (decimal)r.DistanceKm, (string)r.Difficulty,
                (string)r.StartPoint, (string)r.EndPoint, (DateTime)r.CreatedAt));
        }

        public async Task<Route?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT route_id AS Id, name AS Name, description AS Description,
                               distance_km AS DistanceKm, difficulty AS Difficulty,
                               start_point AS StartPoint, end_point AS EndPoint,
                               created_at AS CreatedAt
                        FROM Routes WHERE route_id = @Id";
            var r = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            if (r == null) return null;
            return new Route((int)r.Id, (string)r.Name, (string)(r.Description ?? ""),
                (decimal)r.DistanceKm, (string)r.Difficulty,
                (string)r.StartPoint, (string)r.EndPoint, (DateTime)r.CreatedAt);
        }

        public async Task<int> CreateAsync(Route entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Routes (name, description, distance_km, difficulty, start_point, end_point)
                        OUTPUT INSERTED.route_id
                        VALUES (@Name, @Description, @DistanceKm, @Difficulty, @StartPoint, @EndPoint)";
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                entity.Name,
                entity.Description,
                entity.DistanceKm,
                entity.Difficulty,
                entity.StartPoint,
                entity.EndPoint
            });
        }

        public async Task<bool> UpdateAsync(Route entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Routes SET name = @Name, description = @Description,
                               distance_km = @DistanceKm, difficulty = @Difficulty,
                               start_point = @StartPoint, end_point = @EndPoint
                        WHERE route_id = @Id";
            var rows = await connection.ExecuteAsync(sql, new
            {
                entity.Name,
                entity.Description,
                entity.DistanceKm,
                entity.Difficulty,
                entity.StartPoint,
                entity.EndPoint,
                entity.Id
            });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(
                "DELETE FROM Routes WHERE route_id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}
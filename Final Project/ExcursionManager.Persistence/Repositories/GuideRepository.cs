using Dapper;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;

namespace ExcursionManager.Persistence.Repositories
{
    public class GuideRepository : IRepository<Guide>
    {
        private readonly DatabaseContext _context;

        public GuideRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guide>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT guide_id AS Id, full_name AS FullName, id_number AS IdNumber,
                               specialty AS Specialty, phone AS Phone, email AS Email,
                               is_active AS IsActive, created_at AS CreatedAt
                        FROM Guides ORDER BY full_name";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(g => new Guide(
                (int)g.Id, (string)g.FullName, (string)g.IdNumber,
                (string)(g.Specialty ?? ""), (string)(g.Phone ?? ""),
                (string)(g.Email ?? ""), (bool)g.IsActive, (DateTime)g.CreatedAt));
        }

        public async Task<Guide?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT guide_id AS Id, full_name AS FullName, id_number AS IdNumber,
                               specialty AS Specialty, phone AS Phone, email AS Email,
                               is_active AS IsActive, created_at AS CreatedAt
                        FROM Guides WHERE guide_id = @Id";
            var g = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            if (g == null) return null;
            return new Guide((int)g.Id, (string)g.FullName, (string)g.IdNumber,
                (string)(g.Specialty ?? ""), (string)(g.Phone ?? ""),
                (string)(g.Email ?? ""), (bool)g.IsActive, (DateTime)g.CreatedAt);
        }

        public async Task<int> CreateAsync(Guide entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Guides (full_name, id_number, specialty, phone, email)
                        OUTPUT INSERTED.guide_id
                        VALUES (@FullName, @IdNumber, @Specialty, @Phone, @Email)";
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                entity.FullName,
                entity.IdNumber,
                entity.Specialty,
                entity.Phone,
                entity.Email
            });
        }

        public async Task<bool> UpdateAsync(Guide entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Guides SET full_name = @FullName, specialty = @Specialty,
                               phone = @Phone, email = @Email, is_active = @IsActive
                        WHERE guide_id = @Id";
            var rows = await connection.ExecuteAsync(sql, new
            {
                entity.FullName,
                entity.Specialty,
                entity.Phone,
                entity.Email,
                entity.IsActive,
                entity.Id
            });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(
                "DELETE FROM Guides WHERE guide_id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}

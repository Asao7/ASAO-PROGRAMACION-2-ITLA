using Dapper;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;

namespace ExcursionManager.Persistence.Repositories
{
    public class ParticipantRepository : IRepository<Participant>
    {
        private readonly DatabaseContext _context;

        public ParticipantRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Participant>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT participant_id AS Id, full_name AS FullName,
                               id_number AS IdNumber, age AS Age,
                               emergency_contact AS EmergencyContact,
                               email AS Email, created_at AS CreatedAt
                        FROM Participants ORDER BY full_name";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(p => new Participant(
                (int)p.Id, (string)p.FullName, (string)p.IdNumber, (int)p.Age,
                (string)(p.EmergencyContact ?? ""), (string)(p.Email ?? ""),
                (DateTime)p.CreatedAt));
        }

        public async Task<Participant?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT participant_id AS Id, full_name AS FullName,
                               id_number AS IdNumber, age AS Age,
                               emergency_contact AS EmergencyContact,
                               email AS Email, created_at AS CreatedAt
                        FROM Participants WHERE participant_id = @Id";
            var p = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            if (p == null) return null;
            return new Participant((int)p.Id, (string)p.FullName, (string)p.IdNumber,
                (int)p.Age, (string)(p.EmergencyContact ?? ""),
                (string)(p.Email ?? ""), (DateTime)p.CreatedAt);
        }

        public async Task<int> CreateAsync(Participant entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Participants (full_name, id_number, age, emergency_contact, email)
                        OUTPUT INSERTED.participant_id
                        VALUES (@FullName, @IdNumber, @Age, @EmergencyContact, @Email)";
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                entity.FullName,
                entity.IdNumber,
                entity.Age,
                entity.EmergencyContact,
                entity.Email
            });
        }

        public async Task<bool> UpdateAsync(Participant entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Participants SET full_name = @FullName, age = @Age,
                               emergency_contact = @EmergencyContact, email = @Email
                        WHERE participant_id = @Id";
            var rows = await connection.ExecuteAsync(sql, new
            {
                entity.FullName,
                entity.Age,
                entity.EmergencyContact,
                entity.Email,
                entity.Id
            });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(
                "DELETE FROM Participants WHERE participant_id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}

using Dapper;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;

namespace ExcursionManager.Persistence.Repositories
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly DatabaseContext _context;

        public ReservationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT reservation_id AS Id, participant_id AS ParticipantId,
                               excursion_id AS ExcursionId, reserved_at AS ReservedAt,
                               status AS Status, attended AS Attended, reserved_at AS CreatedAt
                        FROM Reservations ORDER BY reserved_at DESC";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(r => new Reservation(
                (int)r.Id, (int)r.ParticipantId, (int)r.ExcursionId,
                (DateTime)r.ReservedAt, (string)r.Status,
                (bool)r.Attended, (DateTime)r.CreatedAt));
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT reservation_id AS Id, participant_id AS ParticipantId,
                               excursion_id AS ExcursionId, reserved_at AS ReservedAt,
                               status AS Status, attended AS Attended, reserved_at AS CreatedAt
                        FROM Reservations WHERE reservation_id = @Id";
            var r = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            if (r == null) return null;
            return new Reservation((int)r.Id, (int)r.ParticipantId, (int)r.ExcursionId,
                (DateTime)r.ReservedAt, (string)r.Status,
                (bool)r.Attended, (DateTime)r.CreatedAt);
        }

        public async Task<IEnumerable<Reservation>> GetByExcursionAsync(int excursionId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT r.reservation_id AS Id, r.participant_id AS ParticipantId,
                               r.excursion_id AS ExcursionId, r.reserved_at AS ReservedAt,
                               r.status AS Status, r.attended AS Attended,
                               r.reserved_at AS CreatedAt,
                               p.full_name AS ParticipantName
                        FROM Reservations r
                        INNER JOIN Participants p ON r.participant_id = p.participant_id
                        WHERE r.excursion_id = @ExcursionId
                        ORDER BY r.reserved_at";
            var result = await connection.QueryAsync<dynamic>(sql, new { ExcursionId = excursionId });
            return result.Select(r => new Reservation(
                (int)r.Id, (int)r.ParticipantId, (int)r.ExcursionId,
                (DateTime)r.ReservedAt, (string)r.Status,
                (bool)r.Attended, (DateTime)r.CreatedAt));
        }

        public async Task<int> CreateAsync(Reservation entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Reservations (participant_id, excursion_id, status)
                        OUTPUT INSERTED.reservation_id
                        VALUES (@ParticipantId, @ExcursionId, @Status)";
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                entity.ParticipantId,
                entity.ExcursionId,
                entity.Status
            });
        }

        public async Task<bool> UpdateAsync(Reservation entity)
        {
            using var connection = _context.CreateConnection();

            var sql = @"UPDATE Reservations
                        SET participant_id = @ParticipantId,
                            excursion_id = @ExcursionId,
                            status = @Status,
                            attended = @Attended
                        WHERE reservation_id = @Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                entity.ParticipantId,
                entity.ExcursionId,
                entity.Status,
                entity.Attended,
                entity.Id
            });

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(
                "DELETE FROM Reservations WHERE reservation_id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}
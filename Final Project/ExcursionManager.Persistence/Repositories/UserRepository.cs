using Dapper;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Persistence.Context;

namespace ExcursionManager.Persistence.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT user_id AS Id, username AS Username,
                               password_hash AS PasswordHash, full_name AS FullName,
                               email AS Email, role AS Role,
                               is_active AS IsActive, created_at AS CreatedAt
                        FROM Users WHERE username = @Username AND is_active = 1";
            var u = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Username = username });
            if (u == null) return null;
            return new User((int)u.Id, (string)u.Username, (string)u.PasswordHash,
                (string)u.FullName, (string)(u.Email ?? ""), (string)u.Role,
                (bool)u.IsActive, (DateTime)u.CreatedAt);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT user_id AS Id, username AS Username,
                               password_hash AS PasswordHash, full_name AS FullName,
                               email AS Email, role AS Role,
                               is_active AS IsActive, created_at AS CreatedAt
                        FROM Users ORDER BY full_name";
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.Select(u => new User((int)u.Id, (string)u.Username,
                (string)u.PasswordHash, (string)u.FullName,
                (string)(u.Email ?? ""), (string)u.Role,
                (bool)u.IsActive, (DateTime)u.CreatedAt));
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Users (username, password_hash, full_name, email, role)
                        OUTPUT INSERTED.user_id
                        VALUES (@Username, @PasswordHash, @FullName, @Email, @Role)";
            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                user.Username,
                user.PasswordHash,
                user.FullName,
                user.Email,
                user.Role
            });
        }
    }
}
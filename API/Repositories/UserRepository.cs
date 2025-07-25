using API.DAO;
using API.Dtos.User;
using API.Helpers;
using API.Mappers;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly QuizletLiteContext _context;

        public UserRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<User>> GetUsers(UserQuery query)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                var keyword = query.Keyword.ToLower();
                usersQuery = usersQuery.Where(u =>
                    u.Username.ToLower().Contains(keyword) ||
                    u.Email.ToLower().Contains(keyword));
            }

            if (query.Role.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Role == query.Role.Value);
            }

            int totalCount = await usersQuery.CountAsync();

            var users = await usersQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>(users, totalCount, query.Page, query.PageSize);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.Include(u => u.Quizzes).Include(u => u.QuizAttempts).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(UserDto dto)
        {
            var existing = await _context.Users.FindAsync(dto.Id);
            if (existing == null) throw new Exception("User not found");
            dto.MapToExisting(existing); // keeps password the same
            await _context.SaveChangesAsync();

        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExists(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string?> GetAvatarByUserId(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Avatar)
                .FirstOrDefaultAsync();
        }
        public IQueryable<User> GetUsersQueryable()
        {
            return _context.Users.AsQueryable();
        }

    }
}

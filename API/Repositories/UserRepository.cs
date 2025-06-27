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

        public async Task<List<User>> GetUsers(UserQuery query)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(query.Username))
            {
                users = users.Where(u => u.Username.Contains(query.Username));
            }

            if (!string.IsNullOrEmpty(query.Email))
            {
                users = users.Where(u => u.Email.Contains(query.Email));
            }

            if (query.Role.HasValue)
            {
                users = users.Where(u => u.Role == query.Role.Value);
            }

            return await users
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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
    }
}

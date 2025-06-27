using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly QuizletLiteContext _context;

        public AuthRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUser(string email)
        {
            return await _context.Users
                .Include(u => u.Quizzes) // giữ lại nếu User có danh sách Quiz
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users
                .Include(u => u.Quizzes) // nếu muốn trả về cả quizzes
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.PasswordHash = newPassword;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
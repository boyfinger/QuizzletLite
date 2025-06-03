using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly QuizletLiteContext _context;
        
        public AuthenticationRepository(QuizletLiteContext context)
        {
            _context = context;
        }
        public async Task<User?> AuthenticateUser(string email)
        {
            return await _context.Users
                .Include(qr => qr.QuizResults).Include(q => q.Quizzes)
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
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
            return await _context.Users.FindAsync(userId);
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

            user.Password = newPassword;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

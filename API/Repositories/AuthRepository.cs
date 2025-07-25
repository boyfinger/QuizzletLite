using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly QuizletLiteContext _context;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(QuizletLiteContext context, ILogger<AuthRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> AuthenticateUser(string email)
        {
            var user = await _context.Users
                .Include(u => u.Quizzes)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                _logger.LogWarning("No user found with email: {Email}", email);
            else
                _logger.LogInformation("User authenticated successfully: {Email}", email);

            return user;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            _logger.LogInformation("Email existence check for '{Email}': {Exists}", email, exists);
            return exists;
        }

        public async Task<bool> CheckEmailExistsByDifferentId(int id, string email)
        {
            try
            {
                bool exists = await _context.Users
                                        .AnyAsync(u => u.Email == email && u.Id != id);
                if (exists)
                {
                    _logger.LogWarning($"Email '{email}' already exists for a different user (excluding ID: {id}).");
                }
                else
                {
                    _logger.LogInformation($"Email '{email}' is unique or belongs to user ID: {id}.");
                }
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking email '{email}' for different ID: {id}.");
                return true;
            }
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            var exists = await _context.Users.AnyAsync(u => u.Username == username);
            _logger.LogInformation("Username existence check for '{Username}': {Exists}", username, exists);
            return exists;
        }

        public async Task<bool> CheckUsernameExistsByDifferentId(int id, string username)
        {
            try
            {
                bool exists = await _context.Users
                                        .AnyAsync(u => u.Username == username && u.Id != id);
                if (exists)
                {
                    _logger.LogWarning($"Username '{username}' already exists for a different user (excluding ID: {id}).");
                }
                else
                {
                    _logger.LogInformation($"Username '{username}' is unique or belongs to user ID: {id}.");
                }
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking username '{username}' for different ID: {id}.");
                return true;
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user by email '{email}'.");
                return null;
            }
        }

        public async Task<User?> GetUserById(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Quizzes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                _logger.LogWarning("No user found with ID: {UserId}", userId);
            else
                _logger.LogInformation("Retrieved user with ID: {UserId}", userId);

            return user;
        }

        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                var saved = await _context.SaveChangesAsync() > 0;

                if (saved)
                    _logger.LogInformation("New user registered: {Email}", user.Email);
                else
                    _logger.LogWarning("User registration failed (no changes saved): {Email}", user.Email);

                return saved;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during user registration: {Email}", user.Email);
                return false;
            }
        }

        public async Task<bool> UpdateAvatar(int userId, string avatar)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Cannot update avatar – user not found: {UserId}", userId);
                    return false;
                }

                user.Avatar = avatar;
                var saved = await _context.SaveChangesAsync() > 0;

                _logger.LogInformation("Avatar updated for user ID: {UserId}", userId);
                return saved;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating avatar for user ID: {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> UpdatePassword(int userId, string newPasswordHash)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Cannot update password – user not found: {UserId}", userId);
                    return false;
                }

                user.PasswordHash = newPasswordHash;
                var saved = await _context.SaveChangesAsync() > 0;

                _logger.LogInformation("Password updated for user ID: {UserId}", userId);
                return saved;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password for user ID: {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                var result = await _context.SaveChangesAsync() > 0;
                _logger.LogInformation($"User with ID {user.Id} updated successfully.");
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, $"Concurrency conflict when updating user with ID {user.Id}. The user may have been modified by another process.");
                return false;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Database update error when updating user with ID {user.Id}. Message: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while updating user with ID {user.Id}. Message: {ex.Message}");
                return false;
            }
        }
    }
}
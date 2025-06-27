using API.Models;

namespace API.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> AuthenticateUser(string email);

        Task<bool> RegisterUser(User user);

        Task<bool> CheckEmailExists(string email);

        Task<bool> CheckUsernameExists(string username);

        Task<bool> UpdatePassword(int userId, string newPassword);

        Task<User?> GetUserById(int userId);
    }
}

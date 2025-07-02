using API.Models;

namespace API.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> AuthenticateUser(string email);

        Task<bool> RegisterUser(User user);

        Task<bool> CheckEmailExists(string email);

        Task<User?> GetUserByEmail(string email);

        Task<bool> CheckEmailExistsByDifferentId(int id, string email);

        Task<bool> CheckUsernameExists(string username);

        Task<bool> CheckUsernameExistsByDifferentId(int id, string username);

        Task<bool> UpdatePassword(int userId, string newPassword);

        Task<User?> GetUserById(int userId);

        Task<bool> UpdateAvatar(int userId, string avatar);

        Task<bool> UpdateUser(User user);
    }
}

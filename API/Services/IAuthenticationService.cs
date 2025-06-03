using API.Dtos.User;
using API.Models;

namespace API.Services
{
    public interface IAuthenticationService
    {
        Task<User?> AuthenticateUser(LoginDTO loginDTO);

        Task<bool> RegisterUser(RegisterDTO registerDto);

        Task<bool> CheckEmailExists(string email);

        Task<bool> CheckUsernameExists(string username);

        Task<bool> UpdatePassword(int userId, string newPassword);

        Task<User?> GetUserById(int userId);
    }
}

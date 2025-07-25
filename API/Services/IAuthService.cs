using API.Dtos.User;
using API.Models;

namespace API.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateUser(LoginDTO loginDTO);

        Task<bool> RegisterUser(RegisterDTO registerDto);

        Task<bool> CheckEmailExists(string email);
        Task<User?> GetUserByEmail(string email);

        Task<bool> CheckUsernameExists(string username);

        Task<bool> UpdatePasswordInProfile(int userId, string currentPassword, string newPassword, string confirmNewPassword);

        Task<bool> UpdatePasswordInReset(int userId, string newPassword, string confirmNewPassword);

        Task<User?> GetUserById(int userId);
        Task<bool?> ChangeAvatar(int userId, IFormFile avatarFile);
        Task<bool> UpdateUserProfile(UpdateProfileDTO updateProfileDTO);
    }
}

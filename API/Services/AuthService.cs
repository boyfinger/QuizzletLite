using API.Dtos.User;
using API.Hash;
using API.Models;
using API.Repositories;
using API.Utils;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }


        public async Task<User?> AuthenticateUser(LoginDTO loginDTO)
        {
            var user = await _authRepository.AuthenticateUser(loginDTO.Email);

            if (user == null) return null;

            if (!CheckHashed.checkBcrypt(loginDTO.Password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<bool?> ChangeAvatar(int userId, IFormFile avatarFile)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                return false;

            var user = await GetUserById(userId);
            if (user == null) return null;

            var base64Avatar = EncodedString.EncodeFileBase64(avatarFile);
            user.Avatar = await base64Avatar;

            return await _authRepository.UpdateAvatar(userId, user.Avatar);
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _authRepository.CheckEmailExists(email);
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _authRepository.CheckUsernameExists(username);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _authRepository.GetUserByEmail(email);
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _authRepository.GetUserById(userId);
        }

        public async Task<bool> RegisterUser(RegisterDTO registerDto)
        {
            if (!registerDto.Password.Equals(registerDto.ConfirmPassword))
                return false;

            if (await CheckUsernameExists(registerDto.Username))
                return false;

            if (await CheckEmailExists(registerDto.Email))
                return false;

            string hashedImage;
            if (registerDto.Avatar == null)
            {
                hashedImage = RandomStringUtils.GenerateRandomAvatar();
            }
            else
            {
                hashedImage = await EncodedString.EncodeFileBase64(registerDto.Avatar);
            }

            var hashedPassword = EncodedString.HashPassword(registerDto.Password);
            var newUser = new User
            {
                Role = Models.Enums.Role.User,
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                Avatar = hashedImage
            };

            return await _authRepository.RegisterUser(newUser);
        }

        public async Task<bool> UpdatePasswordInProfile(int userId, string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmNewPassword) || string.IsNullOrWhiteSpace(currentPassword))
                return false;

            if (newPassword != confirmNewPassword)
                return false;

            var user = await GetUserById(userId);
            if (user == null)
                return false;

            if (!CheckHashed.checkBcrypt(currentPassword, user.PasswordHash))
                return false;

            var hashedPassword = EncodedString.HashPassword(newPassword);
            return await _authRepository.UpdatePassword(userId, hashedPassword);
        }

        public async Task<bool> UpdatePasswordInReset(int userId, string newPassword, string confirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmNewPassword))
                return false;

            if (newPassword != confirmNewPassword)
                return false;

            var user = await GetUserById(userId);
            if (user == null)
                return false;

            var hashedPassword = EncodedString.HashPassword(newPassword);
            return await _authRepository.UpdatePassword(userId, hashedPassword);
        }

        public async Task<bool> UpdateUserProfile(UpdateProfileDTO updateProfileDTO)
        {
            try
            {
                var user = await _authRepository.GetUserById(updateProfileDTO.UserId);
                if (user == null)
                {
                    return false;
                }
                if (!CheckHashed.checkBcrypt(updateProfileDTO.CurrentPassword, user.PasswordHash))
                {
                    return false; // Incorrect password
                }
                if (user.Email != updateProfileDTO.Email)
                {
                    bool emailExists = await _authRepository.CheckEmailExistsByDifferentId(updateProfileDTO.UserId, updateProfileDTO.Email);
                    if (emailExists)
                    {
                        return false; // Email already taken
                    }
                }
                if (user.Username != updateProfileDTO.Username)
                {
                    bool usernameExists = await _authRepository.CheckEmailExistsByDifferentId(updateProfileDTO.UserId, updateProfileDTO.Username);
                    if (usernameExists)
                    {
                        return false; // Username already taken
                    }
                }
                user.Username = updateProfileDTO.Username;
                user.Email = updateProfileDTO.Email;
                bool updated = await _authRepository.UpdateUser(user);
                if (updated)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

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

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _authRepository.CheckEmailExists(email);
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _authRepository.CheckUsernameExists(username);
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _authRepository.GetUserById(userId);
        }

        public async Task<User?> LoginGoogle()
        {
            return null;
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
                hashedImage = EncodedString.EncodeFileBase64(registerDto.Avatar);
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

        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            var user = await GetUserById(userId);
            if (user == null) return false;

            string newHashedPassword = EncodedString.HashPassword(newPassword);

            return await _authRepository.UpdatePassword(userId, newHashedPassword);
        }

    }
}

using API.Dtos.User;
using API.Hash;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authRepository;

        public AuthenticationService(IAuthenticationRepository authRepository)
        {
            _authRepository = authRepository;
        }


        public async Task<User?> AuthenticateUser(LoginDTO loginDTO)
        {
            var user = await _authRepository.AuthenticateUser(loginDTO.Email);

            if (user == null) return null;

            if (!CheckHashed.checkBcrypt(loginDTO.Password, user.Password))
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

        public async Task<bool> RegisterUser(RegisterDTO registerDto)
        {
            if (!registerDto.Password.Equals(registerDto.ConfirmPassword))
                return false;

            if(await CheckUsernameExists(registerDto.Username)) 
                return false;

            if (await CheckEmailExists(registerDto.Email))
                return false;

            var hashedPassword = EncodedString.HashPassword(registerDto.Password);
            var hashedImage = EncodedString.EncodeFileBase64(registerDto.Avatar);
            var newUser = new User
            {
                RoleId = 2,
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = hashedPassword,
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

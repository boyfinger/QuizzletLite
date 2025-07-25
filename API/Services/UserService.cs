using API.Dtos.User;
using API.Helpers;
using API.Mappers;
using API.Repositories;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuizRepository _quizRepository;

        public UserService(IUserRepository userRepository, IQuizRepository quizRepository)
        {
            _userRepository = userRepository;
            _quizRepository = quizRepository;
        }

        public async Task<PagedResult<UserDto>> GetUsers(UserQuery query)
        {
            var pagedUsers = await _userRepository.GetUsers(query);
            var userDtos = pagedUsers.Items.Select(u => u.ToUserDto()).ToList();

            return new PagedResult<UserDto>(userDtos, pagedUsers.TotalCount, pagedUsers.Page, pagedUsers.PageSize);
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return null;

            var completedCount = await _quizRepository.GetCompletedUniqueQuizCountByUserId(id);
            var yourCount = user.Quizzes.Count;

            return user?.ToUserDtoFull(completedCount, yourCount);
        }

        public async Task AddUser(UserDto userDto)
        {
            // Ví dụ bạn check tồn tại theo username hoặc email
            var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            // Nếu muốn check username riêng thì cũng thêm tương tự

            var user = userDto.ToUser();
            await _userRepository.AddUser(user);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            if (!await _userRepository.UserExists(userDto.Id))
            {
                throw new Exception($"User with ID {userDto.Id} does not exist.");
            }
            await _userRepository.UpdateUser(userDto);
        }

        public async Task<bool> DeleteUser(int id)
        {
            if (!await _userRepository.UserExists(id))
            {
                return false;
            }

            return await _userRepository.DeleteUser(id);
        }

        public async Task<bool> UserExists(int id)
        {
            return await _userRepository.UserExists(id);
        }

        public async Task<string?> GetAvatarByUserId(int userId)
        {
            return await _userRepository.GetAvatarByUserId(userId);
        }
    }
}

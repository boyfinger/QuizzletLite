using API.Dtos.User;
using API.Models;
using API.Utils;

namespace API.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Role = user.Role,
                Email = user.Email,
                Username = user.Username,
                Avatar = user.Avatar
            };
        }
        public static User ToUser(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Role = dto.Role,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = RandomStringUtils.GenerateRandomPassword() // default password when creating new user
            };
        }

        public static void MapToExisting(this UserDto dto, User user)
        {
            user.Role = dto.Role;
            user.Email = dto.Email;
            user.Username = dto.Username;
            // Password remains unchanged
        }
    }
}

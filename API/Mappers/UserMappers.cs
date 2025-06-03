using API.Dtos.User;
using API.Models;

namespace API.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Email = user.Email,
                Username = user.Username,
                RoleName = user.Role?.Role1 ?? "Unknown"
            };
        }
        public static User ToUser(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                RoleId = dto.RoleId,
                Email = dto.Email,
                Username = dto.Username,
                Password = "1" // default password when creating new user
            };
        }

        public static void MapToExisting(this UserDto dto, User user)
        {
            user.RoleId = dto.RoleId;
            user.Email = dto.Email;
            user.Username = dto.Username;
            // Password remains unchanged
        }
    }
}

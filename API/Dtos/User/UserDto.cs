using API.Models.Enums;

namespace API.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; }
        public Role Role { get; set; } 
    }

}

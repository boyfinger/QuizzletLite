namespace API.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public string Avatar { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; }

        public string? RoleName { get; set; }
    }
}

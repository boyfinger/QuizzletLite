using System.ComponentModel.DataAnnotations;
using API.Models.Enums;

namespace API.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public Role Role { get; set; }

        public string? Avatar { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be at least 3 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
    }
}

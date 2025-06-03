using Microsoft.AspNetCore.Mvc;

namespace API.Dtos.User
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}

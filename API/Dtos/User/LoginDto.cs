using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
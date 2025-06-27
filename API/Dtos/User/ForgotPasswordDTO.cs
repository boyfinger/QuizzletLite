using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class ForgotPasswordDTO
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}

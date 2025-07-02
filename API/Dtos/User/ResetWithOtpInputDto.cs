using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class ResetWithOtpInputDto
    {
        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string OtpCode { get; set; } = "";

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; } = "";

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";

    }
}

using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class UpdateProfileDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        // Mật khẩu hiện tại để xác nhận việc cập nhật thông tin profile
        [Required(ErrorMessage = "Your current password is required to confirm changes.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}

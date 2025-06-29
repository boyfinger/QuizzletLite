namespace API.Dtos.User
{
    public class AvatarUploadDTO
    {
        public int UserId { get; set; }
        public IFormFile? Avatar { get; set; }

    }
}

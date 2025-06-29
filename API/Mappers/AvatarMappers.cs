using API.Dtos.User;
using API.Models;

namespace API.Mappers
{
    public static class AvatarMappers
    {
        public static AvatarDTO ToAvatarDto(this User user)
        {
            return new AvatarDTO
            {
                Avatar = user.Avatar
            };
        }

        public static AvatarUploadDTO ToAvatarUploadDto(this User user)
        {
            return new AvatarUploadDTO
            {
                UserId = user.Id,
            };
        }

        public static string ToBase64Avatar(this IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();
            var contentType = file.ContentType;
            return $"data:{contentType};base64,{Convert.ToBase64String(bytes)}";
        }
    }
}

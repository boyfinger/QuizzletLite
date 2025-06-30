namespace API.Hash
{
    public static class EncodedString
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static async Task<string?> EncodeFileBase64(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream); // ✅ Đúng! Phải có await

            byte[] fileBytes = memoryStream.ToArray();
            string? mimeType = file.ContentType;

            string base64String = Convert.ToBase64String(fileBytes);
            return $"data:{mimeType};base64,{base64String}";
        }

    }
}

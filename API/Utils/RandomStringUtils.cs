namespace API.Utils
{
    public static class RandomStringUtils
    {
        public static string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GenerateRandomAvatar()
        {
            var seed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return $"https://api.dicebear.com/7.x/bottts/svg?seed={seed}";
        }

        public static int RandomIntOtp()
        {
            var random = new Random();
            return random.Next(100000, 1000000);
        }
    }
}

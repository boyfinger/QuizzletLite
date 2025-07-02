using Microsoft.Extensions.Caching.Memory;

namespace API.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<OtpService> _logger;

        public OtpService(IMemoryCache cache, ILogger<OtpService> logger)
        {
            _cache = cache;
            _logger = logger;
        }


        public void RemoveOtp(string email)
        {
            var cacheKey = GetCacheKey(email);
            _cache.Remove(cacheKey);
        }

        public void StoreOtp(string email, string otpCode)
        {
            var cacheKey = GetCacheKey(email);
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.Set(cacheKey, otpCode, options);
            _logger.LogInformation("OTP for {email} stored: {otp}", email, otpCode);
        }

        public bool VerifyOtp(string email, string inputOtp)
        {
            var cacheKey = GetCacheKey(email);
            if (_cache.TryGetValue(cacheKey, out string? storedOtp))
            {
                if (storedOtp == inputOtp)
                {
                    _cache.Remove(cacheKey); // OTP đúng → xoá luôn
                    return true;
                }
            }
            return false;
        }

        private string GetCacheKey(string email) => $"OTP_{email.ToLower()}";
    }
}

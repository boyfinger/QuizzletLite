namespace API.Services
{
    public interface IOtpService
    {
        public void StoreOtp(string email, string otpCode);
        public bool VerifyOtp(string email, string inputOtp);
        public void RemoveOtp(string email);
    }
}

namespace API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromEmail, string subject, string message);
        Task SendEmailAsyncToCustomer(string toEmail, string subject, string message);
    }
}

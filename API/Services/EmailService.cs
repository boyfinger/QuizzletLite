
using API.Utils;
using System.Net;
using System.Net.Mail;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string fromEmail, string subject, string message)
        {
            var toEmail = SendMailUtils.receivedEmail; // Địa chỉ email người nhận
            var password = SendMailUtils.password; // Mật khẩu ứng dụng của người gửi (fromEmail)
            var client = new SmtpClient("smtp.gmail.com", 587) // Cổng 587 cho TLS
            {
                EnableSsl = true, // Gmail yêu cầu SSL/TLS
                UseDefaultCredentials = false, // Không dùng thông tin đăng nhập mặc định
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(toEmail, password) // Dùng tài khoản Gmail và mật khẩu ứng dụng
            };


            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true // Nếu bạn muốn gửi email với nội dung HTML
            };
            mailMessage.ReplyToList.Add(new MailAddress(fromEmail));
            mailMessage.To.Add(new MailAddress(toEmail));

            return client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailAsyncToCustomer(string toEmail, string subject, string message)
        {
            try
            {
                var fromEmail = SendMailUtils.receivedEmail; // Email người gửi
                var password = SendMailUtils.password; // Mật khẩu ứng dụng Gmail

                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password))
                {
                    throw new InvalidOperationException("Email hoặc mật khẩu không hợp lệ.");
                }

                if (string.IsNullOrEmpty(toEmail))
                {
                    throw new ArgumentException("Email người nhận không được để trống.");
                }

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(fromEmail, password);

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(fromEmail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = message;
                        mailMessage.IsBodyHtml = true;

                        mailMessage.To.Add(new MailAddress(toEmail));

                        await client.SendMailAsync(mailMessage);
                        Console.WriteLine($"✅ Email đã gửi thành công đến {toEmail}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi gửi email: {ex.Message}");
            }
        }
    }
}

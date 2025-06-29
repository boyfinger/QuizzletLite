using API.DAO;
using API.Hash;
using API.Models;
using API.Models.Enums;
using API.Repositories;
using API.Utils;
using System.Security.Claims;

namespace API.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly QuizletLiteContext _context;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public GoogleAuthService(QuizletLiteContext context, IUserRepository userRepository, IEmailService emailService)
        {
            _context = context;
            _userRepository = userRepository;
            _emailService = emailService;
        }


        public async Task<User> HandleGoogleLoginAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault();
            if (identity == null) throw new InvalidOperationException("Google identity not found.");

            var claims = identity.Claims.ToList();
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "Người dùng";
            var avatar = claims.FirstOrDefault(c => c.Type == "picture" || c.Type == "urn:google:picture")?.Value;

            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("Email not found in Google account.");

            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser != null)
            {
                //existingUser.Avatar = avatar;
                //await _userRepository.UpdateUser(new UserDto
                //{
                //    Id = existingUser.Id,
                //    Username = existingUser.Username,
                //    Email = existingUser.Email,
                //    Avatar = existingUser.Avatar,
                //    Role = existingUser.Role
                //});

                return existingUser;
            }

            var randomPassword = RandomStringUtils.GenerateRandomPassword();
            var hashedPassword = EncodedString.HashPassword(randomPassword);

            var newUser = new User
            {
                Email = email,
                Username = email.Split('@')[0],
                Avatar = avatar,
                PasswordHash = hashedPassword,
                Role = Role.User,
                QuizAttempts = new List<QuizAttempt>(),
                Quizzes = new List<Quiz>()
            };

            await _userRepository.AddUser(newUser);

            await _emailService.SendEmailAsyncToCustomer(
    toEmail: email,
    subject: "Your Login Password",
    message: $"""
    Hello {name},<br/><br/>
    Your login password is: <strong>{randomPassword}</strong><br/><br/>
    Please change your password after logging in to ensure security.<br/><br/>
    Thank you!
"""
);

            return newUser;
        }
    }
}

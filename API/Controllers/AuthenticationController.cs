using API.Dtos.User;
using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly IGoogleAuthService _googleAuthService;

        public AuthenticationController(IAuthService authenticationService, IJwtService jwtService, IGoogleAuthService googleAuthService)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _googleAuthService = googleAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var userLogined = await _authenticationService.AuthenticateUser(loginDTO);
                if (userLogined == null) return Unauthorized("Invalid credentials");

                var userDTO = new UserDto
                {
                    Id = userLogined.Id,
                    Username = userLogined.Username,
                    Email = userLogined.Email,
                    Role = userLogined.Role,
                    Avatar = userLogined.Avatar
                };

                string token = _jwtService.GenerateToken(userLogined);

                var response = new AuthResponseDto
                {
                    UserDto = userDTO,
                    Token = token
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }

        }

        [HttpGet("login-google")]
        public Task SignInWithGoogle()
        {
            //var properties = new AuthenticationProperties
            //{
            //    RedirectUri = Url.Action("GoogleCallback") // gọi callback sau đăng nhập thành công
            //};


            return HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback"),
                Items = { { "prompt", "consent" } }
            });

        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                Console.WriteLine("Google Auth failed: " + result.Failure?.Message);
                return Unauthorized("Google login failed.");
            }


            // Gọi service xử lý đăng nhập Google
            var user = await _googleAuthService.HandleGoogleLoginAsync(result.Principal);

            // Sinh token nếu bạn dùng JWT
            var token = _jwtService.GenerateToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar,
                Role = user.Role
            };

            var response = new AuthResponseDto
            {
                UserDto = userDto,
                Token = token
            };

            return Redirect($"https://localhost:7113/GoogleLoginSuccess" +
                $"?token={token}" +
                $"&id={userDto.Id}" +
                $"&username={Uri.EscapeDataString(userDto.Username)}" +
                $"&email={Uri.EscapeDataString(userDto.Email)}" +
                $"&avatar={Uri.EscapeDataString(userDto.Avatar ?? "")}" +
                $"&role={userDto.Role}");

        }


        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            bool registed = await _authenticationService.RegisterUser(registerDTO);
            return Ok(registed);
        }
    }
}

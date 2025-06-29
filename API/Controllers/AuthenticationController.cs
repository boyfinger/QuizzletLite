using API.Dtos.User;
using API.Mappers;
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

                var userDTO = userLogined.ToUserDto();

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

            var user = await _googleAuthService.HandleGoogleLoginAsync(result.Principal);

            var token = _jwtService.GenerateToken(user);

            var userDto = user.ToUserDto();

            var response = new AuthResponseDto
            {
                UserDto = userDto,
                Token = token
            };

            return Redirect($"https://localhost:7113/Auth/GoogleLoginSuccess" +
                $"?token={token}" +
                $"&id={userDto.Id}" +
                $"&username={Uri.EscapeDataString(userDto.Username)}" +
                $"&email={Uri.EscapeDataString(userDto.Email)}" +
                $"&role={userDto.Role}");

        }

        [HttpPost("get-user-avatar")]
        public async Task<IActionResult> GetUserAvatar([FromBody] AvatarRequestDTO dto)
        {
            var user = await _authenticationService.GetUserById(dto.UserId);
            if (user == null || string.IsNullOrEmpty(user.Avatar))
                return NotFound();

            return Ok(new AvatarDTO { Avatar = user.Avatar });
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            bool registed = await _authenticationService.RegisterUser(registerDTO);
            if (!registed)
                return BadRequest("Registration failed.");
            return Ok(true);
        }

        [HttpPost("change-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ChangeAvatar([FromForm] AvatarUploadDTO avatarUploadDTO)
        {
            if (avatarUploadDTO.Avatar == null || avatarUploadDTO.Avatar.Length == 0)
                return BadRequest("Image null");
            Console.WriteLine(avatarUploadDTO.Avatar);
            bool? success = await _authenticationService.ChangeAvatar(avatarUploadDTO.UserId, avatarUploadDTO.Avatar);
            return Ok(success);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var success = await _authenticationService.UpdatePassword(changePasswordDTO.UserId, changePasswordDTO.NewPassword, changePasswordDTO.ConfirmNewPassword);
            if (!success)
                return BadRequest("Could not update password. User may not exist.");
            return Ok(success);
        }
    }
}

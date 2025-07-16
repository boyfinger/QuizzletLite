using API.Dtos.User;
using API.Mappers;
using API.Services;
using API.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public AuthenticationController(IAuthService authenticationService,
                                        IJwtService jwtService,
                                        IGoogleAuthService googleAuthService,
                                        IUserService userService,
                                        IOtpService otpService,
                                        IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _googleAuthService = googleAuthService;
            _userService = userService;
            _otpService = otpService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var userLogined = await _authenticationService.AuthenticateUser(loginDTO);
                if (userLogined == null)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Email hoặc mật khẩu không đúng."
                    });
                }

                var userDTO = userLogined.ToUserDto();

                string token = _jwtService.GenerateToken(userLogined);
                Console.WriteLine(token);
                var response = new AuthResponseDto
                {
                    Token = token
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token");

                if (!int.TryParse(userIdClaim, out int userId))
                    return BadRequest("Invalid user ID format");

                var user = await _userService.GetUserById(userId);
                if (user == null)
                    return NotFound("User not found");

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Nếu có ILogger, bạn có thể log lại
                // _logger.LogError(ex, "Error fetching current user");

                return StatusCode(500, new { error = "Internal server error", detail = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("avatarforme")]
        public async Task<IActionResult> GetAvatar()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine(userIdClaim);
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized("Missing user ID");

                int userId = int.Parse(userIdClaim); // Có thể bị lỗi nếu giá trị không hợp lệ
                var avatarUrl = await _userService.GetAvatarByUserId(userId);

                if (string.IsNullOrEmpty(avatarUrl))
                    return NotFound("Avatar not found");

                return Ok(new { avatar = avatarUrl });
            }
            catch (Exception ex)
            {
                // Có thể thêm logger nếu cần:
                //_logger.LogError(ex, "Error getting avatar");
                return StatusCode(500, new { error = "Internal server error", detail = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("login-google")]
        public Task SignInWithGoogle()
        {
            return HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback"),
                Items = { { "prompt", "consent" } }
            });

        }

        [AllowAnonymous]
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

                Token = token
            };

            return Redirect($"https://localhost:7113/Auth/GoogleLoginSuccess" +
                $"?token={token}");

        }

        [AllowAnonymous]
        [HttpPost("get-user-avatar")]
        public async Task<IActionResult> GetUserAvatar([FromBody] AvatarRequestDTO dto)
        {
            var user = await _authenticationService.GetUserById(dto.UserId);
            if (user == null || string.IsNullOrEmpty(user.Avatar))
                return NotFound();

            return Ok(new AvatarDTO { Avatar = user.Avatar });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            bool registed = await _authenticationService.RegisterUser(registerDTO);
            if (!registed)
                return BadRequest("Registration failed.");
            return Ok(true);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("change-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ChangeAvatar([FromForm] AvatarUploadDTO avatarUploadDTO)
        {
            Console.WriteLine("Received file:");
            Console.WriteLine($"Name: {avatarUploadDTO.Avatar?.FileName}");
            Console.WriteLine($"Length: {avatarUploadDTO.Avatar?.Length}");

            if (avatarUploadDTO.Avatar == null || avatarUploadDTO.Avatar.Length == 0)
                return BadRequest("Image null");
            bool? success = await _authenticationService.ChangeAvatar(avatarUploadDTO.UserId, avatarUploadDTO.Avatar);
            return Ok(success);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var success = await _authenticationService.UpdatePasswordInProfile(changePasswordDTO.UserId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword, changePasswordDTO.ConfirmNewPassword);
            if (!success)
                return BadRequest("Could not update password. User may not exist.");
            return Ok(success);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateProfileDTO)
        {
            var success = await _authenticationService.UpdateUserProfile(updateProfileDTO);
            if (!success)
            {
                return BadRequest("Can't update profile");
            }
            return Ok(success);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("get-user-dto")]
        public async Task<IActionResult> GetUserDtoById(int userId)
        {
            var userDto = await _userService.GetUserById(userId);
            if (userDto == null)
            {
                return BadRequest("Can't load user");
            }
            return Ok(userDto);
        }

        [HttpPost("send-reset-otp")]
        public async Task<IActionResult> SendResetOtp([FromBody] ForgotPasswordDTO dto)
        {
            var otp = RandomStringUtils.RandomIntOtp().ToString();
            _otpService.StoreOtp(dto.Email, otp);
            await _emailService.SendEmailAsyncToCustomer(dto.Email, "OTP Reset Code", $"Your OTP is: {otp}");
            return Ok("OTP sent.");
        }

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] ResetWithOtpInputDto resetWithOtpInputDto)
        {
            Console.WriteLine($"🔥 Incoming verify-otp with Email: {resetWithOtpInputDto.Email}, OTP: {resetWithOtpInputDto.OtpCode}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState is invalid");
                return BadRequest("Invalid data.");
            }

            var isValid = _otpService.VerifyOtp(resetWithOtpInputDto.Email, resetWithOtpInputDto.OtpCode);
            if (!isValid)
            {
                Console.WriteLine($"❌ OTP verification failed for {resetWithOtpInputDto.Email}");
                return BadRequest("OTP is incorrect or expired.");
            }

            var user = await _authenticationService.GetUserByEmail(resetWithOtpInputDto.Email);
            if (user == null)
            {
                Console.WriteLine($"❌ User not found for {resetWithOtpInputDto.Email}");
                return NotFound("User not found.");
            }

            Console.WriteLine($"✅ User found. Resetting password for userId: {user.Id}");
            await _authenticationService.UpdatePasswordInReset(user.Id, resetWithOtpInputDto.NewPassword, resetWithOtpInputDto.ConfirmPassword);

            _otpService.RemoveOtp(resetWithOtpInputDto.Email);
            Console.WriteLine("✅ Password reset successful, OTP removed.");

            return Ok("Password has been reset.");
        }
    }
}

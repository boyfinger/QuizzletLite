using API.Dtos.User;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var userLogined = await _authenticationService.AuthenticateUser(loginDTO);
            if (userLogined == null) return Unauthorized("Invalid credentials");

            var userDTO = new UserDto
            {
                Id = userLogined.Id,
                Username = userLogined.Username,
                Email = userLogined.Email,
                Role = userLogined.Role,
                Avatar = userLogined.Avatar ?? null
            };

            return Ok(userDTO);
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

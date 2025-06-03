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
        public async Task<IActionResult> Login([FromQuery] LoginDTO loginDTO)
        {
            var userLogined = await _authenticationService.AuthenticateUser(loginDTO);
            if (userLogined != null) {
                return Ok("Logined");
            } else
            {
                return BadRequest("Can't login");
            }

               
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            bool registed = await _authenticationService.RegisterUser(registerDTO);
            return Ok(registed);
        }
    }
}

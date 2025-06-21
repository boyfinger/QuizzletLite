using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult GenerateToken(int userId, string role)
        {
            return Ok(new { token = JWTHelper.GenerateToken(userId.ToString(), role, _configuration) });
        }
    }
}

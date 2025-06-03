using API.Dtos.User;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User?Username=abc&Email=xyz&page=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQuery query)
        {
            var users = await _userService.GetUsers(query);
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto dto)
        {
            try
            {
                await _userService.AddUser(dto);
                return Ok(new { message = "User created successfully with default password '1'." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/User
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto dto)
        {
            try
            {
                await _userService.UpdateUser(dto);
                return Ok(new { message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
            {
                return NotFound(new { message = "User not found." });
            }
            return Ok(new { message = "User deleted successfully." });
        }
    }
}

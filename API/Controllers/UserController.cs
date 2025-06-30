using API.Dtos.User;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

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

        //[Authorize(Policy = "AdminPolicy")]
        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQuery query)
        {
            var pagedResult = await _userService.GetUsers(query);
            return Ok(pagedResult);
        }

        //[Authorize(Policy = "AdminPolicy")]
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

        //[Authorize(Policy = "AdminPolicy")]
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

        //[Authorize(Policy = "AdminPolicy")]
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

        //[Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found." });
                }
                return Ok(new { message = "User deleted successfully." });
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("REFERENCE constraint") == true)
            {
                return BadRequest(new { message = "Cannot delete user because it is referenced by other data." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

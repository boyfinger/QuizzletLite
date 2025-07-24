using API.Dtos.Quiz;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin_QuizController : ODataController
    {
        private readonly Admin_IQuizService _quizService;

        public Admin_QuizController(Admin_IQuizService quizService)
        {
            _quizService = quizService;
        }
        [EnableQuery]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QuizQuery query)
        {
            var quizzes = await _quizService.GetQuizzes(query);
            return Ok(quizzes);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quiz = await _quizService.GetQuizById(id);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Admin_QuizDto dto)
        {
            // Lấy UserId từ Claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID claim not found." });
            }
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid User ID." });
            }
            dto.CreatedBy = userId;

            await _quizService.AddQuiz(dto);
            return Ok(new { message = "Quiz created successfully" });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Admin_QuizDto dto)
        {
            if (id != dto.Id) return BadRequest("Mismatched ID");
            await _quizService.UpdateQuiz(dto);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _quizService.DeleteQuiz(id);
            if (!deleted) return NotFound();
            return Ok();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var toggled = await _quizService.ToggleQuizStatus(id);
            if (!toggled) return NotFound();
            return Ok(new { message = "Quiz status updated successfully." });
        }
    }
}

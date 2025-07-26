using API.Dtos.UserQuiz;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user/quizzes")]
    public class UserQuizzesController : ControllerBase
    {
        private readonly IUserQuizService _quizService;

        public UserQuizzesController(IUserQuizService quizService)
        {
            _quizService = quizService;
        }

        // POST: api/user/quizzes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] UserQuizDto dto)
        {
            int userId = GetCurrentUserId();
            var quizId = await _quizService.CreateUserQuiz(dto, userId);
            return Ok(new { quizId });
        }

        // PUT: api/user/quizzes/{id}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UserQuizDto dto)
        {
            int userId = GetCurrentUserId();

            if (dto.Id == null || dto.Id != id)
                return BadRequest("Quiz ID mismatch.");

            var success = await _quizService.UpdateUserQuiz(dto, userId);
            if (!success) return NotFound("Quiz not found or not owned by user.");

            return NoContent();
        }

        // DELETE: api/user/quizzes/{id}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            int userId = GetCurrentUserId();

            var success = await _quizService.DeleteUserQuiz(id, userId);
            if (!success) return NotFound("Quiz not found or not owned by user.");

            return NoContent();
        }

        // GET: api/user/quizzes/{id}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizDetail(int id)
        {
            int userId = GetCurrentUserId();

            var quiz = await _quizService.GetUserQuizDetail(id, userId);
            if (quiz == null) return NotFound("Quiz not found or not owned by user.");

            return Ok(quiz);
        }

        private int GetCurrentUserId()
        {
            // Tìm claim bằng ClaimTypes.NameIdentifier trước, nếu không có thì tìm claim "nameid"
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Không tìm thấy thông tin người dùng từ token.");

            return int.Parse(userIdClaim);
        }

        // GET: api/user/quizzes
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMyQuizzes()
        {
            int userId = GetCurrentUserId();

            var quizzes = await _quizService.GetUserQuizzes(userId);
            return Ok(quizzes);
        }

        [HttpDelete("{quizId}/questions/{questionId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteQuestion(int quizId, int questionId)
        {
            int userId = GetCurrentUserId();

            var success = await _quizService.DeleteQuestion(quizId, questionId, userId);
            if (!success)
                return NotFound("Question not found or not owned by user.");

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateActiveStatus(int id, [FromBody] QuizActiveStatusDto dto)
        {
            bool updated = await _quizService.UpdateActiveStatusAsync(id, dto.IsActive);
            if (!updated)
                return NotFound("Quiz not found.");

            return NoContent();
        }


    }
}

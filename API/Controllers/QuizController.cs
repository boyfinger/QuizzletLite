using API.Dtos;
using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Mappers;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizService _quizService;

        public QuizController(IQuizRepository quizRepository, IQuizService quizService)
        {
            _quizRepository = quizRepository;
            _quizService = quizService;
        }

        [AllowAnonymous]
        [HttpGet("quizzes")]
        public async Task<IActionResult> GetQuizzes([FromQuery] QuizQuery query)
        {
            var listQuery = await _quizRepository.GetQuizzes(query);
            var list = listQuery.Select(q => q.ToQuizDto());
            return Ok(list);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("DoQuiz")]
        public async Task<IActionResult> DoQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var quizAttempt = await _quizService.ProcessQuizAttempt(submissionDto, userId);
                return Ok(new
                {
                    Score = quizAttempt.Score,
                    Id = quizAttempt.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{quizId}/Questions")]
        public async Task<IActionResult> GetQuizDetails(int quizId)
        {
            try
            {
                return Ok(await _quizService.GetQuizDetailsAsync(quizId));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuizzesDto>> CreateQuiz([FromBody] CreateQuizDto createQuizDto)
        {
            if (createQuizDto == null) return BadRequest("Quiz data is required");
            var quizDto = await _quizService.CreateQuizAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuizById), new { id = quizDto.Id }, quizDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{quizId}")]
        public async Task<ActionResult<QuizzesDto>> UpdateQuiz(int quizId, [FromBody] UpdateQuizDto updateQuizDto)
        {
            if (updateQuizDto == null) return BadRequest("Quiz data is required");
            try
            {
                var quizDto = await _quizService.UpdateQuizAsync(quizId, updateQuizDto);
                return Ok(quizDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{quizId}/deactivate")]
        public async Task<IActionResult> DeactivateQuiz(int quizId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var quiz = await _quizService.GetQuizByIdAsync(quizId);
            if (quiz == null)
                return NotFound("Quiz not found");

            if (quiz.CreatedBy != userId)
                return Forbid("You do not have permission to deactivate this quiz");

            var success = await _quizService.DeactivateQuizAsync(quizId);
            if (!success)
                return StatusCode(500, "Failed to deactivate quiz");

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{quizId}/delete")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var quiz = await _quizRepository.GetQuizByIdWithAttempts(quizId);
            if (quiz == null)
                return NotFound("Quiz not found");

            if (quiz.CreatedBy != userId)
                return Forbid("You do not have permission to delete this quiz");

            if (quiz.QuizAttempts != null && quiz.QuizAttempts.Any())
            {
                // ❌ Có người đã làm quiz → chỉ deactivate
                var success = await _quizRepository.DeactivateQuiz(quizId);
                if (success != true)
                    return StatusCode(500, "Failed to deactivate quiz");

                return Ok("Quiz has attempts and was deactivated.");
            }
            else
            {
                // ✅ Chưa ai làm → xóa hẳn
                var success = await _quizRepository.DeleteQuiz(quizId);
                if (success != true)
                    return StatusCode(500, "Failed to delete quiz");

                return Ok("Quiz deleted successfully.");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<QuizzesDto>>> GetQuizzesByUser(int userId)
        {
            var quizzes = await _quizService.GetQuizzesByUserAsync(userId);
            return Ok(quizzes);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("your-quiz")]
        public async Task<IActionResult> GetYourQuiz([FromQuery] QuizQuery query)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { error = "Token invalid", detail = "User ID claim missing" });
                }

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest(new { error = "Bad Request", detail = "User ID in token is not a valid number" });
                }

                var quizzes = await _quizService.GetUserQuizzesByPage(userId, query);
                if (quizzes == null) return NotFound("No quizzes found");
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", detail = ex.Message });
            }
        }

        [HttpPost("submit")]
        public async Task<ActionResult<double>> SubmitQuiz([FromBody] QuizSubmissionDto submissionDto, int userId)
        {
            if (submissionDto == null || submissionDto.Answers == null) return BadRequest("Submission data is required");
            var score = await _quizService.ProcessQuizAttempt(submissionDto, userId);
            return Ok(score);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizzesDto>> GetQuizById(int id)
        {
            try
            {
                var quiz = await _quizService.GetQuizByIdAsync(id);
                return Ok(quiz);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}

using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizAttemptController : ControllerBase
    {
        private readonly IQuizAttemptService _quizAttemptService;
        private readonly IQuizAttemptRepository _quizAttemptRepository;

        public QuizAttemptController(IQuizAttemptService quizResultService, IQuizAttemptRepository quizResultRepository)
        {
            _quizAttemptService = quizResultService;
            _quizAttemptRepository = quizResultRepository;
        }

        [Authorize]
        [HttpGet("{quizAttemptId}")]
        public async Task<IActionResult> GetResultById([FromRoute] int quizAttemptId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized("User not authenticated.");
                }
                var attempt = await _quizAttemptService.GetQuizAttempt(quizAttemptId);
                if (attempt == null)
                {
                    return NotFound("Quiz attempt not found.");
                }
                if (attempt.UserId != int.Parse(userIdClaim))
                {
                    return Forbid("You do not have permission to view this quiz attempt.");
                }
                return Ok(attempt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetResultsByUserId()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var resultsQuery = await _quizAttemptRepository.GetQuizAttemptsOfUser(userId);
                if (resultsQuery == null || !resultsQuery.Any())
                {
                    return NotFound("No quiz results found for this user.");
                }
                var results = resultsQuery.Select(qr => new
                {
                    qr.Id,
                    qr.QuizId,
                    qr.Quiz.Name,
                    qr.Score,
                    qr.CompletedDate
                }).ToList();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}

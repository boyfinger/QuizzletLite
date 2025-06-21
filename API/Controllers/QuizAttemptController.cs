using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{quizAttemptId}")]
        public async Task<IActionResult> GetResultById([FromRoute] int quizAttemptId)
        {
            try
            {
                var result = await _quizAttemptService.GetQuizAttempt(quizAttemptId);
                if (result == null)
                {
                    return NotFound("Quiz attempt not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetResultsByUserId([FromRoute] int userId)
        {
            try
            {
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

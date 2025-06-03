using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizResultController : ControllerBase
    {
        private readonly IQuizResultService _quizResultService;
        private readonly IQuizResultRepository _quizResultRepository;

        public QuizResultController(IQuizResultService quizResultService, IQuizResultRepository quizResultRepository)
        {
            _quizResultService = quizResultService;
            _quizResultRepository = quizResultRepository;
        }

        [HttpGet("{quizResultId}")]
        public async Task<IActionResult> GetResultById([FromRoute] int quizResultId)
        {
            try
            {
                var result = await _quizResultService.GetQuizResult(quizResultId);
                if (result == null)
                {
                    return NotFound("Quiz result not found.");
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
                var resultsQuery = await _quizResultRepository.GetQuizResultsOfUser(userId);
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

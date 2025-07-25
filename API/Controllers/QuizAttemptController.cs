using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{quizAttemptId}")]
        public async Task<IActionResult> GetResultById([FromRoute] int quizAttemptId)
        {
            try
            {
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"🔍 Received Claim: {claim.Type} = {claim.Value}");
                }
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"User ID Claim: {userIdClaim}");
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user")]
        public async Task<IActionResult> GetResultsByUserId()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine(userIdClaim);
                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user ID in JWT token.");
                }
                var resultsQuery = await _quizAttemptRepository.GetQuizAttemptsOfUser(userId);
                if (resultsQuery == null || !resultsQuery.Any())
                {
                    return NotFound("No quiz results found for this user.");
                }
                var results = resultsQuery.Select(qr => new
                {
                    qr.Id,
                    qr.QuizId,
                    QuizName = qr.Quiz.Name,
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("api/statistics/unique-quiz-users")]
        public IActionResult GetDistinctUserCount()
        {
            var count = _quizAttemptService.CountDistinctUsersParticipated();
            return Ok(count);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("top5-users")]
        public IActionResult GetTop5Users()
        {
            var topUsers = _quizAttemptService.GetTop5UsersByScore();
            var result = topUsers.Select(u => new { Username = u.Username, TotalScore = u.TotalScore }).ToList();
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("most-active-user")]
        public async Task<IActionResult> GetMostActiveUser()
        {
            var result = await _quizAttemptService.GetMostActiveUserAsync();
            if (result == null)
                return NotFound("No attempts found");

            return Ok(new { Username = result.Value.Username, AttemptCount = result.Value.AttemptCount });
        }
    }
}

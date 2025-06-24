using API.Dtos.Quiz;
using API.Dtos;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Mappers;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetQuizzes([FromQuery] QuizQuery query)
        {
            var listQuery = await _quizRepository.GetQuizzes(query);
            var list = listQuery.Select(q => q.ToQuizDto());
            return Ok(list);
        }

        [Authorize]
        [HttpPost("DoQuiz")]
        public async Task<IActionResult> DoQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                return Ok(await _quizService.ProcessQuizAttempt(submissionDto, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
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
        [HttpPatch("{quizId}/deactivate")]
        public async Task<IActionResult> DeactivateQuiz(int quizId)
        {
            var success = await _quizService.DeactivateQuizAsync(quizId);
            if (!success) return NotFound("Quiz not found");
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<QuizzesDto>>> GetQuizzesByUser(int userId)
        {
            var quizzes = await _quizService.GetQuizzesByUserAsync(userId);
            return Ok(quizzes);
        }

        [HttpPost("submit")]
        public async Task<ActionResult<double>> SubmitQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            if (submissionDto == null || submissionDto.Answers == null) return BadRequest("Submission data is required");
            var score = await _quizService.ProcessQuizAttempt(submissionDto);
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

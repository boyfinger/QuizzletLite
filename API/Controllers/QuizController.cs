using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Mappers;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetQuizzes([FromQuery] QuizQuery query)
        {
            var listQuery = await _quizRepository.GetQuizzes(query);
            var list = listQuery.Select(q => q.ToQuizDto());
            return Ok(list);
        }

        [HttpPost("DoQuiz")]
        public async Task<IActionResult> DoQuiz([FromBody] QuizSubmissionDto submissionDto)
        {
            try
            {
                return Ok(await _quizService.ProcessQuizAttempt(submissionDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{quizId}/Questions")]
        public async Task<IActionResult> GetQuizDetails(int quizId)
        {
            var quiz = await _quizRepository.GetQuizById(quizId);
            if (quiz == null)
            {
                return NotFound();
            }
            var quizDto = quiz.ToQuizDetailsDto();
            return Ok(quizDto);
        }
    }
}

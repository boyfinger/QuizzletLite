using API.Dtos.Quiz;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin_QuizController : ControllerBase
    {
        private readonly Admin_IQuizService _quizService;

        public Admin_QuizController(Admin_IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QuizQuery query)
        {
            var quizzes = await _quizService.GetQuizzes(query);
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quiz = await _quizService.GetQuizById(id);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Admin_QuizDto dto)
        {
            await _quizService.AddQuiz(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Admin_QuizDto dto)
        {
            if (id != dto.Id) return BadRequest("Mismatched ID");
            await _quizService.UpdateQuiz(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _quizService.DeleteQuiz(id);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}

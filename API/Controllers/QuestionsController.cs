using API.Dtos.Question;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: api/Questions
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _questionService.GetListAllQuestions();
            return Ok(questions);
        }

        // GET: api/Questions/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var question = await _questionService.GetByIdAsync(id);
            if (question == null)
                return NotFound();
            return Ok(question);
        }

        // POST: api/Questions
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create(QuestionDto dto)
        {
            var created = await _questionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/Questions/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDto>> Update(int id, QuestionDto dto)
        {
            var updated = await _questionService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        // DELETE: api/Questions/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _questionService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }


    }
}

using API.Dtos.Quiz;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin_QuestionController : ControllerBase
    {
        private readonly Admin_IQuestionService _service;

        public Admin_QuestionController(Admin_IQuestionService service)
        {
            _service = service;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("by-quiz/{quizId}")]
        public async Task<IActionResult> GetByQuizId(int quizId)
        {
            var questions = await _service.GetQuestionsByQuizId(quizId);
            return Ok(questions);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _service.GetById(id);
            if (question == null) return NotFound();
            return Ok(question);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Admin_QuestionDto dto)
        {
            await _service.Add(dto);
            return Ok(new { message = "Question created successfully." });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Admin_QuestionDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _service.Update(dto);
            return Ok(new { message = "Question updated successfully." });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok(new { message = "Question deleted successfully." });
        }
    }
}

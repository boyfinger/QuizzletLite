using API.Dtos.Question;
using API.Mappers;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionDtosController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionService _questionService;

        public QuestionDtosController(IQuestionRepository questionRepository, IQuestionService questionService)
        {
            _questionRepository = questionRepository;
            _questionService = questionService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [EnableQuery]
        public IQueryable<QuestionDto> Get()
        {
            var query = _questionRepository.GetAllQuestions();
            return query.Select(QuestionMappers.ToQuestionDto).AsQueryable();
        }
    }
}

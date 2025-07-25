using API.Dtos;
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
    public class QuizzesDtoController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizService _quizService;

        public QuizzesDtoController(IQuizRepository quizRepository, IQuizService quizService)
        {
            _quizRepository = quizRepository;
            _quizService = quizService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableQuery]
        [HttpGet]
        public IQueryable<QuizzesDto> Get()
        {
            var query = _quizRepository.GetAllQuizzes(); // IQueryable<Quiz>
            return query.Select(QuizMappers.MapToDtoExpr);
        }
    }
}

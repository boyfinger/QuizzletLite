using API.Dtos.QuizAttempt;
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
    public class QuizAttemptDtosController : ControllerBase
    {
        private readonly IQuizAttemptService _quizAttemptService;
        private readonly IQuizAttemptRepository _quizAttemptRepository;

        public QuizAttemptDtosController(IQuizAttemptService quizResultService, IQuizAttemptRepository quizResultRepository)
        {
            _quizAttemptService = quizResultService;
            _quizAttemptRepository = quizResultRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [EnableQuery]
        public IQueryable<QuizAttemptDto> Get([FromQuery] bool includeQuestions = false)
        {
            var query = _quizAttemptRepository.GetAllQuizAttempts();
            if (includeQuestions)
            {
                return query.Select(QuizAttemptMappers.MapToQuizAttemptEpr);
            }
            else
            {
                return query.Select(QuizAttemptMappers.MapToQuizAttemptLiteExpr);
            }
        }
    }
}

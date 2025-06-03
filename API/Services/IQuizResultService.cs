using API.Dtos.QuizResult;

namespace API.Services
{
    public interface IQuizResultService
    {
        Task<QuizResultDto> GetQuizResult(int quizResultId);
    }
}

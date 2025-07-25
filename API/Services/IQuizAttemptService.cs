using API.Dtos.QuizAttempt;

namespace API.Services
{
    public interface IQuizAttemptService
    {
        IQueryable<QuizAttemptDto> GetAllQuizAttempts();
        Task<QuizAttemptDto> GetQuizAttempt(int quizResultId);
    }
}

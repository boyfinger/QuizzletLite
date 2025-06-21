using API.Dtos.QuizAttempt;

namespace API.Services
{
    public interface IQuizAttemptService
    {
        Task<QuizAttemptDto> GetQuizAttempt(int quizResultId);
    }
}

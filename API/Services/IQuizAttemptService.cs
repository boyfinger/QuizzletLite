using API.Dtos.QuizAttempt;

namespace API.Services
{
    public interface IQuizAttemptService
    {
        IQueryable<QuizAttemptDto> GetAllQuizAttempts();
        Task<QuizAttemptDto> GetQuizAttempt(int quizResultId);
        int CountDistinctUsersParticipated();
        List<(string Username, double TotalScore)> GetTop5UsersByScore();
        Task<(string Username, int AttemptCount)?> GetMostActiveUserAsync();
    }
}

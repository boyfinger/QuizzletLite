using API.Models;

namespace API.Repositories
{
    public interface IQuizAttemptRepository
    {
        IQueryable<QuizAttempt> GetAllQuizAttempts();
        public Task<QuizAttempt?> GetQuizAttemptById(int quizAttemptId);
        Task<List<QuizAttempt>> GetQuizAttemptsOfUser(int userId);
        Task<QuizAttempt> SaveQuizAttempt(QuizAttempt quizAttempt);
        int CountDistinctUsersParticipated();
        List<(string Username, double TotalScore)> GetTop5UsersByScore();
        Task<(string Username, int AttemptCount)?> GetMostActiveUserAsync();
    }
}

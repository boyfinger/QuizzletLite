using API.Models;

namespace API.Repositories
{
    public interface IQuizAttemptRepository
    {
        public Task<QuizAttempt?> GetQuizAttemptById(int quizAttemptId);
        Task<List<QuizAttempt>> GetQuizAttemptsOfUser(int userId);
        Task<QuizAttempt> SaveQuizAttempt(QuizAttempt quizAttempt);
    }
}

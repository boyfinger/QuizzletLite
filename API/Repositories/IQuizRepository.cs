using API.Helpers;
using API.Models;

namespace API.Repositories
{
    public interface IQuizRepository
    {
        IQueryable<Quiz> GetAllQuizzes();
        Task<List<Quiz>> GetQuizzes(QuizQuery query);
        Task<Quiz?> GetQuizById(int quizId);
        Task<Quiz?> GetQuizByIdWithDetails(int quizId);
        Task<Quiz?> GetQuizByIdWithAttempts(int quizId);
        Task<List<Quiz>> GetQuizzesByUser(int userId);
        Task<Quiz> CreateQuiz(Quiz quiz);
        Task<Quiz> UpdateQuiz(Quiz quiz);
        Task<bool> DeactivateQuiz(int quizId);
        Task<bool?> DeleteQuiz(int quizId);
        Task<int> GetCompletedUniqueQuizCountByUserId(int userId);
    }
}

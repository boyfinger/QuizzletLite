using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Models;

namespace API.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetQuizzes(QuizQuery query);
        Task<Quiz?> GetQuizById(int quizId);
        Task<Quiz?> GetQuizByIdWithDetails(int quizId);
        Task<List<Quiz>> GetQuizzesByUser(int userId);
        Task<Quiz> CreateQuiz(Quiz quiz);
        Task<Quiz> UpdateQuiz(Quiz quiz);
        Task<bool> DeactivateQuiz(int quizId);
    }
}

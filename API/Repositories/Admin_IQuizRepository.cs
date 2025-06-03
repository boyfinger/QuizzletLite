using API.Helpers;
using API.Models;

namespace API.Repositories
{
    public interface Admin_IQuizRepository
    {
        Task<List<Quiz>> GetQuizzes(QuizQuery query);
        Task<Quiz?> GetQuizById(int id);
        Task AddQuiz(Quiz quiz);
        Task UpdateQuiz(Quiz quiz);
        Task<bool> DeleteQuiz(int id);
        Task<bool> QuizExists(int id);
    }
}

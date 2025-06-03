using API.Models;

namespace API.Repositories
{
    public interface IQuizResultRepository
    {
        public Task<QuizResult?> GetQuizResultById(int quizResultId);
        Task<List<QuizResult>> GetQuizResultsOfUser(int userId);
    }
}

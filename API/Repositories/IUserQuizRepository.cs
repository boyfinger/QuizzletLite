using API.Models;
using API.Dtos.UserQuiz;

namespace API.Repositories
{
    public interface IUserQuizRepository
    {
        Task<int> CreateUserQuiz(UserQuizDto dto, int userId);
        Task<bool> UpdateUserQuiz(UserQuizDto dto, int userId);
        Task<bool> DeleteUserQuiz(int quizId, int userId);
        Task<Quiz?> GetQuizById(int quizId, int userId);
        Task<IEnumerable<QuizDto>> GetUserQuizzesAsync(int userId);
        Task<bool> DeleteQuestionAsync(int quizId, int questionId);
        Task<Quiz?> GetByIdAsync(int id);
        Task<bool> UpdateActiveStatusAsync(int quizId, bool isActive);

    }
}

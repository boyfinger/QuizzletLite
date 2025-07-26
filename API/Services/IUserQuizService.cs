using API.Dtos.UserQuiz;
using API.Models;

namespace API.Services
{
    public interface IUserQuizService
    {
        Task<int> CreateUserQuiz(UserQuizDto dto, int userId);
        Task<bool> UpdateUserQuiz(UserQuizDto dto, int userId);
        Task<bool> DeleteUserQuiz(int quizId, int userId);
        Task<Quiz?> GetUserQuizDetail(int quizId, int userId);
        Task<IEnumerable<QuizDto>> GetUserQuizzes(int userId);
        Task<bool> DeleteQuestion(int quizId, int questionId, int userId);
        Task<bool> UpdateActiveStatusAsync(int quizId, bool isActive);
    }
}

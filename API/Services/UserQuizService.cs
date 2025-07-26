using API.Dtos.UserQuiz;
using API.Repositories;
using API.Models;

namespace API.Services
{
    public class UserQuizService : IUserQuizService
    {
        private readonly IUserQuizRepository _repo;

        public UserQuizService(IUserQuizRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateUserQuiz(UserQuizDto dto, int userId)
        {
            return await _repo.CreateUserQuiz(dto, userId);
        }

        public async Task<bool> UpdateUserQuiz(UserQuizDto dto, int userId)
        {
            return await _repo.UpdateUserQuiz(dto, userId);
        }

        public async Task<bool> DeleteUserQuiz(int quizId, int userId)
        {
            return await _repo.DeleteUserQuiz(quizId, userId);
        }

        public async Task<Quiz?> GetUserQuizDetail(int quizId, int userId)
        {
            return await _repo.GetQuizById(quizId, userId);
        }
        public async Task<IEnumerable<QuizDto>> GetUserQuizzes(int userId)
        {
            return await _repo.GetUserQuizzesAsync(userId);
        }
        public async Task<bool> DeleteQuestion(int quizId, int questionId, int userId)
        {
            return await _repo.DeleteQuestionAsync(quizId, questionId);
        }
        public async Task<bool> UpdateActiveStatusAsync(int quizId, bool isActive)
        {
            return await _repo.UpdateActiveStatusAsync(quizId, isActive);
        }
    }
}

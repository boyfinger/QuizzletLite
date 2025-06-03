using API.Dtos.Quiz;
using API.Helpers;

namespace API.Services
{
    
    
        public interface Admin_IQuizService
        {
            Task<List<QuizDto>> GetQuizzes(QuizQuery query);
            Task<QuizDto?> GetQuizById(int id);
            Task AddQuiz(Admin_QuizDto quizDto);
            Task UpdateQuiz(Admin_QuizDto quizDto);
            Task<bool> DeleteQuiz(int id);
        }
    
}

using API.Dtos.Quiz;
using API.Dtos.User;
using API.Helpers;

namespace API.Services
{
    
    
        public interface Admin_IQuizService
        {
        Task<PagedResult<Admin_QuizDto>> GetQuizzes(QuizQuery query);
        Task<QuizDto?> GetQuizById(int id);
            Task AddQuiz(Admin_QuizDto quizDto);
            Task UpdateQuiz(Admin_QuizDto quizDto);
            Task<bool> DeleteQuiz(int id);
            Task<bool> ToggleQuizStatus(int id);
        IQueryable<Admin_QuizDto> GetQuizzesForOData();

    }

}

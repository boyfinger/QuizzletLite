using API.Dtos.Quiz;

namespace API.Services
{
    public interface Admin_IQuestionService
    {
        Task<IEnumerable<Admin_QuestionDto>> GetQuestionsByQuizId(int quizId);
        Task<Admin_QuestionDto?> GetById(int id);
        Task Add(Admin_QuestionDto dto);
        Task Update(Admin_QuestionDto dto);
        Task Delete(int id);
    }
}

using API.Dtos.Question;

namespace API.Services
{
    public interface IQuestionService
    {
        Task<List<QuestionDto>> GetListAllQuestions();
        IQueryable<QuestionDto> GetAllQuestions();
        Task<QuestionDto?> GetByIdAsync(int id);
        Task<QuestionDto> CreateAsync(QuestionDto dto);
        Task<QuestionDto> UpdateAsync(int id, QuestionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

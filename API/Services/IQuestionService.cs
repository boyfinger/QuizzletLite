using API.Dtos.Question;

namespace API.Services
{
    public interface IQuestionService
    {
        Task<List<QuestionFullDto>> GetListAllQuestions();
        IQueryable<QuestionFullDto> GetAllQuestions();
        Task<QuestionFullDto?> GetByIdAsync(int id);
        Task<QuestionFullDto> CreateAsync(QuestionFullDto dto);
        Task<QuestionFullDto> UpdateAsync(int id, QuestionFullDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

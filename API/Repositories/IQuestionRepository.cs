using API.Models;

namespace API.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetListAllQuestion();
        IQueryable<Question> GetAllQuestions();
        Task<Question?> GetByIdAsync(int id);
        Task<Question> CreateAsync(Question question);
        Task<Question> UpdateAsync(int id, Question question);
        Task<bool> DeleteAsync(int id);
    }
}

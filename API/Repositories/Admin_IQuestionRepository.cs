using API.Models;

namespace API.Repositories
{
    public interface Admin_IQuestionRepository
    {
        Task<IEnumerable<Question>> GetQuestionsByQuizId(int quizId);
        Task<Question?> GetById(int id);
        Task Add(Question question);
        Task Update(Question question);
        Task Delete(Question question);
    }
}

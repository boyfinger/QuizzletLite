using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizletLiteContext _context;

        public QuestionRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public IQueryable<Question> GetAllQuestions()
        {
            return _context.Questions.AsNoTracking();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateAsync(int id, Question question)
        {
            var existing = await _context.Questions.FindAsync(id);
            if (existing == null) throw new KeyNotFoundException("Question not found");

            // Update fields
            existing.Content = question.Content;
            existing.OptionsJson = question.OptionsJson;
            existing.QuestionType = question.QuestionType;
            existing.IsActive = question.IsActive;
            existing.QuizId = question.QuizId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Questions.FindAsync(id);
            if (existing == null) return false;

            _context.Questions.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Question>> GetListAllQuestion()
        {
            return await _context.Questions.ToListAsync();
        }
    }
}

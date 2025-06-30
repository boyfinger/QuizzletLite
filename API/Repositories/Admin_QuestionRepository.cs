using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Repositories
{
    public class Admin_QuestionRepository : Admin_IQuestionRepository
    {
        private readonly QuizletLiteContext _context;

        public Admin_QuestionRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetQuestionsByQuizId(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
        }

        public async Task<Question?> GetById(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task Add(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Question question)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }
}

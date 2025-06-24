using API.DAO;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizletLiteContext _context;

        public QuizRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<Quiz> CreateQuiz(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> DeactivateQuiz(int quizId)
        {
            var quiz = await GetQuizById(quizId);
            if (quiz == null) return false;

            quiz.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Quiz?> GetQuizById(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .Include(q => q.CreatedByNavigation)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<Quiz?> GetQuizByIdWithDetails(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<List<Quiz>> GetQuizzes(QuizQuery query)
        {
            var list = _context.Quizzes.Where(q => (bool)q.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                list = list.Where(q => q.Name.Contains(query.Name));
            }

            list = list.Skip((query.Page - 1) * query.PageSize)
                       .Take(query.PageSize);

            return await list.Include(q => q.CreatedByNavigation).ToListAsync();
        }

        public async Task<List<Quiz>> GetQuizzesByUser(int userId)
        {
            return await _context.Quizzes
                .Where(q => q.CreatedBy == userId)
                .ToListAsync();
        }

        public async Task<bool> SaveQuizAttempt(QuizAttempt quizAttempt)
        {
            try
            {
                await _context.QuizAttempts.AddAsync(quizAttempt);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Quiz> UpdateQuiz(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }
    }
}

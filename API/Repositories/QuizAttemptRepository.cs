using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class QuizAttemptRepository : IQuizAttemptRepository
    {
        private readonly QuizletLiteContext _context;

        public QuizAttemptRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public IQueryable<QuizAttempt> GetAllQuizAttempts()
        {
            return _context.QuizAttempts.AsNoTracking();
        }

        public async Task<QuizAttempt?> GetQuizAttemptById(int quizAttemptId)
        {
            return await _context.QuizAttempts
                .Include(qa => qa.User)
                .Include(qa => qa.Quiz)
                .FirstOrDefaultAsync(qa => qa.Id == quizAttemptId);
        }

        public async Task<List<QuizAttempt>> GetQuizAttemptsOfUser(int userId)
        {
            return await _context.QuizAttempts
                .Include(qa => qa.User)
                .Include(qa => qa.Quiz)
                .Where(qa => qa.UserId == userId)
                .ToListAsync();
        }

        public async Task<QuizAttempt> SaveQuizAttempt(QuizAttempt quizAttempt)
        {
            await _context.QuizAttempts.AddAsync(quizAttempt);
            await _context.SaveChangesAsync();
            return quizAttempt;
        }
    }
}

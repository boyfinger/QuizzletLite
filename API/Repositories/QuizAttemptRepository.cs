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
    }
}

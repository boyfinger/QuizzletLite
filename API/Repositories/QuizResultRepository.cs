using API.DAO;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class QuizResultRepository : IQuizResultRepository
    {
        private readonly QuizletLiteContext _context;

        public QuizResultRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<QuizResult?> GetQuizResultById(int quizResultId)
        {
            return await _context.QuizResults
                .Include(qr => qr.Quiz)
                .Include(qr => qr.SelectedAnswers)
                .FirstOrDefaultAsync(qr => qr.Id == quizResultId);
        }

        public async Task<List<QuizResult>> GetQuizResultsOfUser(int userId)
        {
            return await _context.QuizResults
                .Where(qr => qr.UserId == userId)
                .Include(qr => qr.Quiz)
                .ToListAsync();
        }
    }
}

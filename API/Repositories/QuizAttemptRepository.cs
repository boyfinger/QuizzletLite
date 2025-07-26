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
        public int CountDistinctUsersParticipated()
        {
            return _context.QuizAttempts
                           .Select(a => a.UserId)
                           .Distinct()
                           .Count();
        }
        public List<(string Username, double TotalScore)> GetTop5UsersByScore()
        {
            var result = _context.QuizAttempts
                .Where(q => q.User != null)
                .GroupBy(q => new { q.UserId, q.User.Username })
                .Select(g => new
                {
                    g.Key.Username,
                    TotalScore = g.Max(x => x.Score)
                })
                .OrderByDescending(x => x.TotalScore)
                .Take(5)
                .ToList();

            return result.Select(r => (r.Username, r.TotalScore)).ToList();
        }
        public async Task<(string Username, int AttemptCount)?> GetMostActiveUserAsync()
        {
            var result = await _context.QuizAttempts
                .GroupBy(a => a.UserId)
                .Select(g => new {
                    UserId = g.Key,
                    AttemptCount = g.Count()
                })
                .OrderByDescending(g => g.AttemptCount)
                .Join(_context.Users,
                      g => g.UserId,
                      u => u.Id,
                      (g, u) => new { u.Username, g.AttemptCount })
                .FirstOrDefaultAsync();

            return result != null ? (result.Username, result.AttemptCount) : null;
        }
    }
}

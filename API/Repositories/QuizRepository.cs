using API.DAO;
using API.Dtos.Quiz.QuizSubmission;
using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizletLiteContext _context;

        public QuizRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestionsOfQuiz(int quizId)
        {
            return await _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizById(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.CreatedByNavigation)
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

        public async Task<bool> SaveQuizAttempt(QuizSubmissionDto submissionDto, double score)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var quizAttempt = new QuizResult
                {
                    UserId = submissionDto.UserId,
                    QuizId = submissionDto.QuizId,
                    Score = score,
                    CompletedDate = DateTime.UtcNow
                };
                _context.QuizResults.Add(quizAttempt);
                await _context.SaveChangesAsync();
                foreach (var answer in submissionDto.Answers)
                {
                    var quizAnswer = new SelectedAnswer
                    {
                        QuizResultId = quizAttempt.Id,
                        QuestionId = answer.QuestionId,
                        AnswerId = answer.AnswerId
                    };
                    _context.SelectedAnswers.Add(quizAnswer);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}

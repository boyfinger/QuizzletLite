using API.DAO;
using API.Dtos.UserQuiz;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Repositories
{
    public class UserQuizRepository : IUserQuizRepository
    {
        private readonly QuizletLiteContext _context;

        public UserQuizRepository(QuizletLiteContext context)
        {
            _context = context;
        }

        public async Task<int> CreateUserQuiz(UserQuizDto dto, int userId)
        {
            var quiz = new Quiz
            {
                Name = dto.Name,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Questions = dto.Questions.Select(q => new Question
                {
                    Content = q.Content,
                    QuestionType = q.QuestionType,
                    IsActive = true,
                    OptionsJson = JsonConvert.SerializeObject(q.Options)
                }).ToList()
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;
        }

        public async Task<bool> UpdateUserQuiz(UserQuizDto dto, int userId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == dto.Id && q.CreatedBy == userId);

            if (quiz == null) return false;

            quiz.Name = dto.Name;
            quiz.Questions.Clear(); // delete old questions

            foreach (var q in dto.Questions)
            {
                quiz.Questions.Add(new Question
                {
                    Content = q.Content,
                    QuestionType = q.QuestionType,
                    IsActive = true,
                    OptionsJson = JsonConvert.SerializeObject(q.Options)
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserQuiz(int quizId, int userId)
        {
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == quizId && q.CreatedBy == userId);

            if (quiz == null) return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Quiz?> GetQuizById(int quizId, int userId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId && q.CreatedBy == userId);
        }
        public async Task<IEnumerable<QuizDto>> GetUserQuizzesAsync(int userId)
        {
            return await _context.Quizzes
                .Where(q => q.CreatedBy == userId)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    CreatedOn = q.CreatedOn,
                    IsActive = q.IsActive,
                })
                .ToListAsync();
        }
        public async Task<bool> DeleteQuestionAsync(int quizId, int questionId)
        {
            // Tìm câu hỏi theo id và quizId để đảm bảo câu hỏi thuộc quiz đó
            var question = await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == questionId && q.QuizId == quizId);

            if (question == null)
                return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _context.Quizzes.FindAsync(id);
        }

        public async Task<bool> UpdateActiveStatusAsync(int quizId, bool isActive)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null) return false;

            quiz.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

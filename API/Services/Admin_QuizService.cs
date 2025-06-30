using API.Dtos.Quiz;
using API.Helpers;
using API.Mappers;
using API.Models;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    
        public class Admin_QuizService : Admin_IQuizService
        {
            private readonly Admin_IQuizRepository _quizRepo;

            public Admin_QuizService(Admin_IQuizRepository quizRepo)
            {
                _quizRepo = quizRepo;
            }

        public async Task<PagedResult<Admin_QuizDto>> GetQuizzes(QuizQuery query)
        {
            var quizzesQuery = _quizRepo.GetQuizzesQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                quizzesQuery = quizzesQuery.Where(q => q.Name.Contains(query.Name));
            }
            if (query.IsActive.HasValue)  // thêm phần này để filter theo trạng thái
            {
                quizzesQuery = quizzesQuery.Where(q => q.IsActive == query.IsActive.Value);
            }

            var total = await quizzesQuery.CountAsync();

            var quizzes = await quizzesQuery
                .OrderByDescending(q => q.CreatedOn)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var quizDtos = quizzes.Select(q => new Admin_QuizDto
            {
                Id = q.Id,
                Name = q.Name,
                CreatedBy = q.CreatedBy,
                CreatedByName = q.CreatedByNavigation?.Username,
                CreatedOn = q.CreatedOn,
                IsActive = q.IsActive
            }).ToList();

            return new PagedResult<Admin_QuizDto>(
                quizDtos,
                total,
                query.Page,
                query.PageSize
            );
        }

        public async Task<QuizDto?> GetQuizById(int id)
            {
                var quiz = await _quizRepo.GetQuizById(id);
                return quiz?.ToQuizDto();
            }

            public async Task AddQuiz(Admin_QuizDto quizDto)
            {
            

            var quiz = quizDto.ToQuiz();
                quiz.CreatedOn = DateTime.UtcNow;
            quiz.CreatedBy = 3;
                quiz.IsActive = true;
                await _quizRepo.AddQuiz(quiz);
            }

            public async Task UpdateQuiz(Admin_QuizDto quizDto)
            {
                var quiz = await _quizRepo.GetQuizById(quizDto.Id);
                if (quiz == null) throw new Exception("Quiz not found");

                quiz.Name = quizDto.Name;
                quiz.IsActive = quizDto.IsActive;
                await _quizRepo.UpdateQuiz(quiz);
            }

            public async Task<bool> DeleteQuiz(int id)
            {
                return await _quizRepo.DeleteQuiz(id);
            }
        public async Task<bool> ToggleQuizStatus(int id)
        {
            var quiz = await _quizRepo.GetQuizById(id);
            if (quiz == null) return false;

            quiz.IsActive = !quiz.IsActive;
            await _quizRepo.UpdateQuiz(quiz);
            return true;
        }
    }
    
}

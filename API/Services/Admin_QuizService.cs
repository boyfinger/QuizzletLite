using API.Dtos.Quiz;
using API.Helpers;
using API.Mappers;
using API.Repositories;

namespace API.Services
{
    
        public class Admin_QuizService : Admin_IQuizService
        {
            private readonly Admin_IQuizRepository _quizRepo;

            public Admin_QuizService(Admin_IQuizRepository quizRepo)
            {
                _quizRepo = quizRepo;
            }

            public async Task<List<QuizDto>> GetQuizzes(QuizQuery query)
            {
                var quizzes = await _quizRepo.GetQuizzes(query);
                return quizzes.Select(q => q.ToQuizDto()).ToList();
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
        }
    
}

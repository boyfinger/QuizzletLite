using API.Dtos.Quiz;
using API.Models;

namespace API.Mappers
{
    public static class Admin_QuizMappers
    {
        public static Admin_QuizDto ToAdminQuizDto(this Quiz quiz)
        {
            return new Admin_QuizDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedBy = quiz.CreatedBy,
                CreatedByName = quiz.CreatedByNavigation?.Username,
                CreatedOn = quiz.CreatedOn,
                IsActive = quiz.IsActive
            };
        }

        public static Quiz ToQuiz(this Admin_QuizDto dto)
        {
            return new Quiz
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedBy = dto.CreatedBy,
                CreatedOn = dto.CreatedOn,
                IsActive = dto.IsActive
            };
        }

        public static void MapToExisting(this Admin_QuizDto dto, Quiz quiz)
        {
            quiz.Name = dto.Name;
            quiz.IsActive = dto.IsActive;
            // Do not update CreatedBy or CreatedOn to preserve data integrity
        }
    }
}

using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizDetails;
using API.Models;

namespace API.Mappers
{
    public static class QuizMappers
    {
        public static QuizDto ToQuizDto(this Quiz quiz)
        {
            return new QuizDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedByUserName = quiz.CreatedByNavigation?.Username ?? "Unknown"
            };
        }

        public static QuizDetailsDto ToQuizDetailsDto(this Quiz quiz)
        {
            return new QuizDetailsDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedByUsername = quiz.CreatedByNavigation?.Username ?? "Unknown",
                Questions = quiz.Questions.Select(
                    q => new QuizQuestionsDto
                    {
                        Id = q.Id,
                        Content = q.Content,
                        QuestionTypeId = q.QuestionTypeId,
                        Answers = q.Answers.Select(a => new QuestionOptionDto
                        {
                            Id = a.Id,
                            Content = a.Content,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    }
                ).ToList()
            };
        }
    } 
}

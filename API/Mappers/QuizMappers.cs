using API.Dtos.Quiz;
using API.Dtos.Quiz.QuizDetails;
using API.Models;
using Newtonsoft.Json;

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
                Questions = quiz.Questions.Select(q => new QuizQuestionsDto
                {
                    Id = q.Id,
                    Content = q.Content,
                    Answers = JsonConvert.DeserializeObject<List<QuestionOptionDto>>(q.OptionsJson)
                }).ToList()
            };
        }
    }
}

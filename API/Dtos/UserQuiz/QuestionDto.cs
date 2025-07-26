using API.Dtos.Quiz.QuizDetails;
using API.Models.Enums;

namespace API.Dtos.UserQuiz
{
    public class QuestionDto
    {
        public string Content { get; set; } = null!;
        public QuestionType QuestionType { get; set; }
        public List<QuestionOptionDto> Options { get; set; } = new();
    }
}

using API.Models.Enums;

namespace API.Dtos.Quiz.QuizDetails
{
    public class QuizQuestionsDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public QuestionType QuestionType { get; set; }
        public List<QuestionOptionDto> Options { get; set; } = new List<QuestionOptionDto>();
    }
}

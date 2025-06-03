namespace API.Dtos.Quiz.QuizDetails
{
    public class QuizQuestionsDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionTypeId { get; set; }
        public List<QuestionOptionDto> Answers { get; set; } = new List<QuestionOptionDto>();
    }
}

namespace API.Dtos.Quiz
{
    public class Admin_QuestionDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int QuizId { get; set; }
        public string OptionsJson { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}

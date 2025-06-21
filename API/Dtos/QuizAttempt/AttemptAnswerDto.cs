namespace API.Dtos.QuizAttempt
{
    public class AttemptAnswerDto
    {
        public string Content { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrect { get; set; }
    }
}

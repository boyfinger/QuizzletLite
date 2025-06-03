namespace API.Dtos.QuizResult
{
    public class ResultAnswerDto
    {
        public int AnswerId { get; set; }
        public string Content { get; set; }
        public bool UserSelected { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}

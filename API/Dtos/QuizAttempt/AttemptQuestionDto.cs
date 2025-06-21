namespace API.Dtos.QuizAttempt
{
    public class AttemptQuestionDto
    {
        public string QuestionContent { get; set; }
        public List<AttemptAnswerDto> Answers { get; set; }
        public bool IsCorrect { get; set; }
    }
}

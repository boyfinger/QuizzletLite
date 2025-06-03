namespace API.Dtos.QuizResult
{
    public class ResultQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public List<ResultAnswerDto> Answers { get; set; }
        public bool IsCorrect { get; set; }
    }
}

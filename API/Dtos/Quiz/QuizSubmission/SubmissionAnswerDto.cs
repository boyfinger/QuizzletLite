namespace API.Dtos.Quiz.QuizSubmission
{
    public class SubmissionAnswerDto
    {
        public int QuestionId { get; set; }
        public List<string> SelectedAnswers { get; set; } = new List<String>();
    }
}

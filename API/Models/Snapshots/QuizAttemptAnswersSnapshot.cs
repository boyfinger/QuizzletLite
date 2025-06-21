namespace API.Models.Snapshots
{
    public class QuizAttemptAnswersSnapshot
    {
        public string? QuestionContent { get; set; }
        public List<QuestionOptionsSnapshot> Options { get; set; } = new List<QuestionOptionsSnapshot>();
        public List<string> SelectedAnswers { get; set; } = new List<string>();
    }
}

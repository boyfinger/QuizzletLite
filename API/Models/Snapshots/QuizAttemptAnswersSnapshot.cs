namespace API.Models.Snapshots
{
    public class QuizAttemptAnswersSnapshot
    {
        public string? Content { get; set; }
        public List<QuestionOptionsSnapshot> Options { get; set; } = new List<QuestionOptionsSnapshot>();
        public List<string> Selected { get; set; } = new List<string>();
    }
}

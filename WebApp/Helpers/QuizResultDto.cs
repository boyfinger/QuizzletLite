namespace WebApp.Helpers
{
    public class QuizResultDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizName { get; set; } = string.Empty;
        public double Score { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}

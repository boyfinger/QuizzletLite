namespace API.Dtos.QuizAttempt
{
    public class QuizAttemptDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int UserId { get; set; }

        public List<AttemptQuestionDto> Questions { get; set; }
        public double TotalScore { get; set; }
    }
}

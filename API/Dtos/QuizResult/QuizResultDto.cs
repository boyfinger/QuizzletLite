namespace API.Dtos.QuizResult
{
    public class QuizResultDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int UserId { get; set; }

        public List<ResultQuestionDto> Questions { get; set; }
        public double TotalScore { get; set; }
    }
}

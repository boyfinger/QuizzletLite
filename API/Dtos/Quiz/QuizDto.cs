namespace API.Dtos.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CreatedByUserName { get; set; } = null!;
        public int NumberOfQuestions { get; set; }
    }
}

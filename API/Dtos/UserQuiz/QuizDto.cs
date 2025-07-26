namespace API.Dtos.UserQuiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
    }

}

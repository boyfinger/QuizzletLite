namespace API.Dtos.Quiz
{
    public class UpdateQuizDto
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}

namespace API.Dtos.Quiz
{
    public class CreateQuizDto
    {
        public string Name { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

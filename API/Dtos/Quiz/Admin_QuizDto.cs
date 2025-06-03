namespace API.Dtos.Quiz
{
    public class Admin_QuizDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CreatedBy { get; set; }

        public string? CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
    }
}

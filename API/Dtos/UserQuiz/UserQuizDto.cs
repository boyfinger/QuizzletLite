namespace API.Dtos.UserQuiz
{
    public class UserQuizDto
    {
        public int? Id { get; set; } // null khi tạo mới
        public string Name { get; set; } = null!;
        public List<QuestionDto> Questions { get; set; } = new();
    }
}

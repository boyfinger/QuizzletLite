using API.Models.Enums;

namespace API.Dtos.Question
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string OptionsJson { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public bool IsActive { get; set; }

        // Optional: Nếu bạn muốn expose thêm thông tin Quiz
        public int QuizId { get; set; }
        public string? QuizName { get; set; } // Chỉ nếu cần thêm

    }
}

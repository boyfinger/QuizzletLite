namespace API.Dtos.Quiz.QuizDetails
{
    public class QuizDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUsername { get; set; }
        public List<QuizQuestionsDto> Questions { get; set; }
    }
}

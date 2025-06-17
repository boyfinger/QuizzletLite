using API.Models.Enums;
namespace API.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int QuizId { get; set; }

    public string OptionsJson { get; set; } = null!;

    public QuestionType QuestionType { get; set; }

    public virtual Quiz? Quiz { get; set; }
}

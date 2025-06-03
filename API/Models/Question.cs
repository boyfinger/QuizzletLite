using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public partial class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int QuizId { get; set; }

    public int QuestionTypeId { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual QuestionType? QuestionType { get; set; }

    public virtual Quiz? Quiz { get; set; }

    public virtual ICollection<SelectedAnswer> SelectedAnswers { get; set; } = new List<SelectedAnswer>();
}

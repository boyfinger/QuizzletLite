using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public partial class QuizResult
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuizId { get; set; }

    public DateTime? CompletedDate { get; set; }

    public double Score { get; set; }

    public virtual Quiz? Quiz { get; set; }

    public virtual ICollection<SelectedAnswer> SelectedAnswers { get; set; } = new List<SelectedAnswer>();

    public virtual User? User { get; set; }
}

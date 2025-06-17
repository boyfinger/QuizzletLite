using System;
using System.Collections.Generic;

namespace API.Models;

public partial class QuizAttempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuizId { get; set; }

    public DateTime? CompletedDate { get; set; }

    public double Score { get; set; }

    public string QuizName { get; set; } = null!;

    public string AnswersJson { get; set; } = null!;

    public virtual Quiz? Quiz { get; set; }

    public virtual User? User { get; set; }
}

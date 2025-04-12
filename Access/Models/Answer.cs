using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Answer
{
    public int Id { get; set; }

    public string AnswerText { get; set; } = null!;

    public bool Correct { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}

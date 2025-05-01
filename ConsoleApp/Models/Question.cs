using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Question
{
    public int Id { get; set; }

    public string QuestionText { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}

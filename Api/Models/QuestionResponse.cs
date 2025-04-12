using System.Collections.Generic;

namespace Api.Models;

public class QuestionResponse
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = null!;
    public List<AnswerResponse> Answers { get; set; } = new();
}

public class AnswerResponse
{
    public int AnswerId { get; set; }
    public string AnswerText { get; set; } = null!;
    public bool Correct { get; set; }
}

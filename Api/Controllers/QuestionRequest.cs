namespace Api.Controllers;

public class QuestionRequest
{
    public string QuestionText { get; set; }
    public List<AnswerRequest> Answers { get; set; } = new();
}
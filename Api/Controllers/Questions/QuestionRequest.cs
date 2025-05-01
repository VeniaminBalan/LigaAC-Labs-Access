using Api.Controllers.Answers;

namespace Api.Controllers.Questions;

public class QuestionRequest
{
    public string QuestionText { get; set; }
    public List<AnswerRequest> Answers { get; set; } = new();
}
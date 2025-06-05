using System.Text.Json.Serialization;

namespace QuizClient.Models;

public class QuestionRequest
{
    [JsonPropertyName("questionText")]
    public string QuestionText { get; set; } = null!;
    
    [JsonPropertyName("answers")]
    public List<AnswerRequest> Answers { get; set; } = new();
}

public class AnswerRequest
{
    [JsonPropertyName("answerText")]
    public string AnswerText { get; set; } = null!;
    
    [JsonPropertyName("correct")]
    public bool Correct { get; set; }
}

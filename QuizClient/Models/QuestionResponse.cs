using System.Text.Json.Serialization;

namespace QuizClient.Models;

public class QuestionResponse
{
    [JsonPropertyName("questionId")]
    public int QuestionId { get; set; }
    
    [JsonPropertyName("questionText")]
    public string QuestionText { get; set; } = null!;
    
    [JsonPropertyName("answers")]
    public List<AnswerResponse> Answers { get; set; } = new();
}

public class AnswerResponse
{
    [JsonPropertyName("answerId")]
    public int AnswerId { get; set; }
    
    [JsonPropertyName("answerText")]
    public string AnswerText { get; set; } = null!;
    
    [JsonPropertyName("correct")]
    public bool Correct { get; set; }
}

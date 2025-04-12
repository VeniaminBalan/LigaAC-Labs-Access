using Api.Models;
using System.Net.Http;
using System.Text.Json;

namespace Api.Services;

public class QuestionService
{
    private readonly HttpClient _httpClient;

    public QuestionService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<QuestionResponse>> GetQuestionsAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5020/api/Questions");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<QuestionResponse>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<QuestionResponse>();
    }
}

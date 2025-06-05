using System.Net.Http.Json;
using QuizClient.Models;

namespace QuizClient.Services;

public class QuizApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public QuizApiService(string baseUrl = "http://localhost:5020")
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl)
        };
    }

    /// <summary>
    /// Retrieves all questions from the API
    /// </summary>
    /// <returns>A list of all questions with their answers</returns>
    public async Task<List<QuestionResponse>> GetAllQuestionsAsync()
    {
        try
        {
            // Since there's no direct endpoint for all questions, we'll use a very large page size
            // In a production environment, you might want to implement pagination handling
            var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<QuestionResponse>>("/api/Questions?pageSize=1000");
            
            if (response == null || response.Items == null)
            {
                return new List<QuestionResponse>();
            }

            return response.Items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving questions: {ex.Message}");
            return new List<QuestionResponse>();
        }
    }

    /// <summary>
    /// Creates a new question with its answers
    /// </summary>
    /// <param name="questionRequest">The question data to create</param>
    /// <returns>The created question response or null if failed</returns>
    public async Task<QuestionResponse?> CreateQuestionAsync(QuestionRequest questionRequest)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Questions", questionRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error creating question: {response.StatusCode}");
                return null;
            }
            
            return await response.Content.ReadFromJsonAsync<QuestionResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating question: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieves a specific question by its ID
    /// </summary>
    /// <param name="id">The ID of the question to retrieve</param>
    /// <returns>The question response or null if not found</returns>
    public async Task<QuestionResponse?> GetQuestionByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<QuestionResponse>($"/api/Questions/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"Question with ID {id} not found.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving question: {ex.Message}");
            return null;
        }
    }
}

public class PaginatedResponse<T>
{
    [System.Text.Json.Serialization.JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();
    
    [System.Text.Json.Serialization.JsonPropertyName("page")]
    public int Page { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}

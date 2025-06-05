using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using QuizMCP.Models;

namespace QuizMCP.Services
{
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

        public async Task<PaginatedResponse<QuestionResponse>> GetQuestionsAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<QuestionResponse>>($"api/Questions?page={page}&pageSize={pageSize}");
                return response ?? new PaginatedResponse<QuestionResponse>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving questions: {ex.Message}");
                return new PaginatedResponse<QuestionResponse>();
            }
        }

        public async Task<QuestionResponse?> GetQuestionByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<QuestionResponse>($"api/Questions/{id}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving question {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<QuestionResponse>> GetRandomQuestionsAsync(int count = 10)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<QuestionResponse>>($"api/Questions/random?count={count}");
                return response ?? new List<QuestionResponse>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving random questions: {ex.Message}");
                return new List<QuestionResponse>();
            }
        }
    }
}

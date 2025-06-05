using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using QuizMCP.Services;
using QuizMCP.Models;
using System.Text.Json;

// Create a generic host builder for
// dependency injection, logging, and configuration.
var builder = Host.CreateApplicationBuilder(args);

// Configure logging for better integration with MCP clients.
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Register the MCP server and configure it to use stdio transport.
// Scan the assembly for tool definitions.
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

// Register the QuizApiService for dependency injection
builder.Services.AddSingleton<QuizApiService>();

// Build and run the host. This starts the MCP server.
await builder.Build().RunAsync();

// Define a static class to hold MCP tools.
[McpServerToolType]
public static class EchoTool
{
    // Expose a tool that echoes the input message back to the client.
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"Hello from C#: {message}";

    // Expose a tool that returns the input message in reverse.
    [McpServerTool, Description("Echoes in reverse the message sent by the client.")]
    public static string ReverseEcho(string message) => new string(message.Reverse().ToArray());
}

// Define a class to hold Quiz-related MCP tools.
[McpServerToolType]
public class QuizTool
{
    private readonly QuizApiService _quizApiService;
    private readonly ILogger<QuizTool> _logger;

    public QuizTool(QuizApiService quizApiService, ILogger<QuizTool> logger)
    {
        _quizApiService = quizApiService;
        _logger = logger;
    }

    [McpServerTool, Description("Retrieves all questions from the Questions API with pagination.")]
    public async Task<string> GetQuestions(int page = 1, int pageSize = 10)
    {
        _logger.LogInformation($"Retrieving questions with page={page}, pageSize={pageSize}");
        
        try
        {
            var response = await _quizApiService.GetQuestionsAsync(page, pageSize);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving questions");
            return $"Error retrieving questions: {ex.Message}";
        }
    }

    [McpServerTool, Description("Retrieves a specific question by ID from the Questions API.")]
    public async Task<string> GetQuestion(int id)
    {
        _logger.LogInformation($"Retrieving question with id={id}");
        
        try
        {
            var question = await _quizApiService.GetQuestionByIdAsync(id);
            return question != null 
                ? JsonSerializer.Serialize(question, new JsonSerializerOptions { WriteIndented = true }) 
                : "Question not found";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving question {id}");
            return $"Error retrieving question: {ex.Message}";
        }
    }

    [McpServerTool, Description("Retrieves random questions from the Questions API.")]
    public async Task<string> GetRandomQuestions(int count = 10)
    {
        _logger.LogInformation($"Retrieving {count} random questions");
        
        try
        {
            var questions = await _quizApiService.GetRandomQuestionsAsync(count);
            return JsonSerializer.Serialize(questions, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving random questions");
            return $"Error retrieving random questions: {ex.Message}";
        }
    }
}
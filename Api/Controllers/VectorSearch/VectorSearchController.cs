using System.Diagnostics.CodeAnalysis;
using Api.Models.VectorSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.VectorData;
using Microsoft.Identity.Client;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;

namespace Api.Controllers.VectorSearch;

[Route("api/[controller]")]
[ApiController]
[Experimental("SKEXP0001")]
public class VectorSearchController : ControllerBase
{
    private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService;
    private readonly IVectorStoreRecordCollection<string, Text> _collection;
    private readonly IChatCompletionService _chatCompletionService;


    [Experimental("SKEXP0001")]
    public VectorSearchController(
        ITextEmbeddingGenerationService textEmbeddingGenerationService, 
        IVectorStore vectorStore, 
        IChatCompletionService chatCompletionService)
    {
        _textEmbeddingGenerationService = textEmbeddingGenerationService;
        _collection = vectorStore.GetCollection<string, Text>("texts");
        _chatCompletionService = chatCompletionService;
    }


    [HttpGet]
    public async Task<IActionResult> Get(string query)
    {
        ReadOnlyMemory<float> searchVector = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(query);
        var searchResult = await _collection.VectorizedSearchAsync(searchVector);
        
        List<VectorResponse> vectorResponse = new List<VectorResponse>();
        await foreach (var record in searchResult.Results)
        {
            vectorResponse.Add(new VectorResponse
            {
                Score = record.Score,
                DocumentPath = record.Record.DocumentPath,
                Content = record.Record.ActualText[..Math.Min(100, record.Record.ActualText.Length)]
            });
        }

        return Ok(vectorResponse);
        
    }

    [HttpPost("chat")]
    public async Task<IActionResult> GetByPrompt([FromBody]string prompt) 
    {
        ReadOnlyMemory<float> searchVector = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(prompt);
        var searchResult = await _collection.VectorizedSearchAsync(searchVector);

        var chat = new ChatHistory("You are helpfull assistent");

        await foreach (var record in searchResult.Results)
        {

            if (record.Score < 5)
                continue;
            chat.AddMessage(AuthorRole.Assistant ,record.Record.ActualText);
        }

        chat.AddUserMessage(prompt);

        var response = await _chatCompletionService.GetChatMessageContentAsync(chat);

        return Ok(response.Content);
    }

}

class VectorResponse
{
    public double? Score { get; set; }
    public string DocumentPath { get; set; }
    public string Content { get; set; }
}
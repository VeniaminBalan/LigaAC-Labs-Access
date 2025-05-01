using System.Diagnostics.CodeAnalysis;
using Api.Models.VectorSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;

namespace Api.Controllers.VectorSearch;

[Route("api/[controller]")]
[ApiController]
[Experimental("SKEXP0001")]
public class VectorSearchController : ControllerBase
{
    private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService;
    private readonly IVectorStoreRecordCollection<string, Text> _collection;
    
    [Experimental("SKEXP0001")]
    public VectorSearchController(ITextEmbeddingGenerationService textEmbeddingGenerationService, IVectorStore vectorStore)
    {
        _textEmbeddingGenerationService = textEmbeddingGenerationService;
        _collection = vectorStore.GetCollection<string, Text>("texts");
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

}

class VectorResponse
{
    public double? Score { get; set; }
    public string DocumentPath { get; set; }
    public string Content { get; set; }
}
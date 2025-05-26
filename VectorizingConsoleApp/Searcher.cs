using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;
using System.Numerics;

namespace SemanticKernelWithGithubModels
{
#pragma warning disable SKEXP0001
    public class Searcher
    {
        private readonly ITextEmbeddingGenerationService _embeddingService;
        private readonly IVectorStoreRecordCollection<string, Text> _collection;

        public Searcher(ITextEmbeddingGenerationService embeddingService, IVectorStoreRecordCollection<string, Text> collection)
        {
            _embeddingService = embeddingService;
            _collection = collection;
        }

        public async Task RunSearchLoopAsync()
        {

            Console.WriteLine("Your awesome search engine is ready!");
            Console.Write("What are you looking for:");

            string? question;
            while (!string.IsNullOrEmpty(question = Console.ReadLine()))
            {
                ReadOnlyMemory<float> searchVector = await _embeddingService.GenerateEmbeddingAsync(question);

                Console.WriteLine();
                Console.Write("Here is your question vector: ");
                Console.Write($"[{searchVector.Span[0]}, {searchVector.Span[1]}, ..., ");
                Console.WriteLine($"{searchVector.Span[searchVector.Length - 2]}, {searchVector.Span[searchVector.Length - 1]}]");


                var searchResult = await _collection.VectorizedSearchAsync(searchVector);
                await foreach (var record in searchResult.Results)
                {
                    Console.WriteLine($"{record.Score} - {record.Record.DocumentPath} - {record.Record.ActualText[..Math.Min(100, record.Record.ActualText.Length)]}");
                }
                Console.WriteLine();
                Console.Write("What are you looking for:");
            }
        }
    }
#pragma warning restore SKEXP0001
}
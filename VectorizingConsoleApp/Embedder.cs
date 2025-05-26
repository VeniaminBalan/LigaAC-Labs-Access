using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;

namespace SemanticKernelWithGithubModels
{
#pragma warning disable SKEXP0001
    public class Embedder(ITextEmbeddingGenerationService embeddingService, IVectorStoreRecordCollection<string, Text> collection, string folder)
    {
        private readonly ITextEmbeddingGenerationService _embeddingService = embeddingService;
        private readonly IVectorStoreRecordCollection<string, Text> _collection = collection;
        private readonly string _folder = folder;

        public async Task<List<Text>> LoadAndEmbedTextsAsync()
        {
            List<Text> texts = [];
            if (!Directory.Exists(_folder))
                return texts;

            foreach (string file in Directory.EnumerateFiles(_folder, "*.txt"))
            {
                try
                {
                    FileInfo info = new(file); 
                    Text? existingText =  await _collection.GetAsync(info.Name);
                    
                    if (existingText != null && existingText.LastModified >= info.LastWriteTimeUtc)
                    {
                        Console.WriteLine($"File {file} is already embedded and up to date.");
                        continue;
                    }

                    var text = await File.ReadAllTextAsync(file);
                    var textObj = new Text
                    {
                        Id = info.Name,
                        LastModified = info.LastWriteTimeUtc,
                        ActualText = text,
                        DocumentPath = file,
                    };
                    textObj.DescriptionEmbedding = (await _embeddingService.GenerateEmbeddingsAsync([textObj.ActualText])).First();
                    texts.Add(textObj);

                    await _collection.UpsertAsync(textObj);

                    Console.WriteLine($"Embedding file: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load text from {file}: {ex.Message}");
                }
            }
            return texts;
        }
    }
#pragma warning restore SKEXP0001
}
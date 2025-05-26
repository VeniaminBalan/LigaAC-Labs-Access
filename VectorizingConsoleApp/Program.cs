using System.ClientModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using OpenAI;
using SemanticKernelWithGithubModels;

#region Configiration

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .Build();


string githubPAT = configuration["VectorSearchEngineOptions:GithubPAT"] ?? throw new ArgumentNullException("Github PAT is not set up");
//"Cohere-embed-v3-multilingual";
string modelId = configuration["VectorSearchEngineOptions:ModelId"] ?? throw new ArgumentNullException("Model Id is not set up");
//https://models.inference.ai.azure.com
string uri = configuration["VectorSearchEngineOptions:Uri"] ?? throw new ArgumentNullException("Uri is not set up");

string redisConnectionString = configuration["RedisOptions:Url"] ?? throw new ArgumentNullException("Redis connection string is not set up");

Console.WriteLine("Initializing your awesome search engine is ready!");

// create client
var client = new OpenAIClient(new ApiKeyCredential(githubPAT), new OpenAIClientOptions { Endpoint = new Uri(uri) });

#pragma warning disable SKEXP0010, SKEXP0001
// Create a chat completion service
Kernel kernel = Kernel
    .CreateBuilder()
    .AddOpenAITextEmbeddingGeneration(modelId, client)
    .AddRedisVectorStore(redisConnectionString)
    .Build();

var textEmbeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
var vectorStore = kernel.GetRequiredService<IVectorStore>();

IVectorStoreRecordCollection<string, Text> collection = vectorStore.GetCollection<string, Text>("texts");
await collection.CreateCollectionIfNotExistsAsync();

// Hardcoded folder path for hotel data
string folder = Path.Combine(".\\files");

#endregion

var embedder = new Embedder(textEmbeddingService, collection, folder);
await embedder.LoadAndEmbedTextsAsync();

var searcher = new Searcher(textEmbeddingService, collection);
await searcher.RunSearchLoopAsync();

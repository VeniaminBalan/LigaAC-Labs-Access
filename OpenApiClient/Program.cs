
using Microsoft.SemanticKernel;
using OpenAI;
using Azure;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.Configuration;
using OpenApiClient.Services;
using OpenApiClient.Models;

// Setup configuration
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .Build();

// Create configuration service
var configService = new ConfigurationService(configuration);

// Get configuration values
string key = configService.GetApiKey();
string model = configService.GetModelId();
var options = configService.GetOpenAIClientOptions();

// Create OpenAI client
var client = new OpenAIClient(new AzureKeyCredential(key), options);

// Build the kernel with OpenAI chat completion
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model, client)
    .Build();

// Get chat completion service from the kernel
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Create and run the chat service
var chatService = new ChatService(chatCompletionService);
await chatService.RunChatLoopAsync();
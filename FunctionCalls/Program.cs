using System.Text;
using Azure;
using FunctionCalls;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI;


string enpoint = "https://models.github.ai/inference";
string key = "";
string model = "openai/gpt-4o-mini";


var options = new OpenAIClientOptions
{
    Endpoint = new Uri(enpoint),
};

var client = new OpenAIClient(new AzureKeyCredential(key), options);

var builder = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model, client);

builder.Plugins.AddFromType<QuizPlugin>();

var kernel = builder.Build();

//await kernel.ImportPluginFromOpenApi("weatherForecast", new Uri("http//localhost"));

var executionSettings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
};

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var chat = new ChatHistory("You are a usefull assistent");

while (true)
{
    Console.WriteLine("You: ");
    var prompt = Console.ReadLine();

    if (prompt == null)
    {
        break;
    }

    chat.AddUserMessage(prompt);

    var response = await chatCompletionService.GetChatMessageContentAsync(chat, executionSettings, kernel);

    Console.WriteLine($"AI: {response.Content}");
    chat.AddAssistantMessage(response.Content);
    Console.WriteLine();
}
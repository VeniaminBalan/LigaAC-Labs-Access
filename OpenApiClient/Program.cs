
using Microsoft.SemanticKernel;
using OpenAI;
using Azure;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;


IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .Build();

string key = configuration["VectorSearchEngineOptions:GithubPAT"] ?? throw new ArgumentNullException("Github PAT is not set up");
//"Cohere-embed-v3-multilingual";
string model = configuration["VectorSearchEngineOptions:ModelId"] ?? throw new ArgumentNullException("Model Id is not set up");
//https://models.inference.ai.azure.com
string enpoint = configuration["VectorSearchEngineOptions:Uri"] ?? throw new ArgumentNullException("Uri is not set up");

static double CalculateProbability(double probLog) => Math.Round(Math.Exp(probLog) * 100, 2);


var options = new OpenAIClientOptions
{
    Endpoint = new Uri(enpoint),
}; 


var client = new OpenAIClient(new AzureKeyCredential(key), options);

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model,client)
    .Build();


var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var chat = new ChatHistory("You are a expert in Geography");

var promptSettings = new OpenAIPromptExecutionSettings
{
    ResponseFormat = typeof(ExportResponse)
};

var promptSettings2 = new OpenAIPromptExecutionSettings
{
    Logprobs = true,
    TopLogprobs = 3
};

while (true) 
{
    Console.WriteLine("Write a question: ");
    var prompt = Console.ReadLine();

    if (prompt == null) 
    {
        break;
    }

    chat.AddUserMessage(prompt);

    var stringBuilder = new StringBuilder();
    await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(chat, promptSettings2)) 
    {
        Console.Write(response);
        stringBuilder.Append(response);
        
        await Task.Delay(100);
        
    }
    chat.AddAssistantMessage(stringBuilder.ToString());
    Console.WriteLine();
}

public class ExportResponse 
{
    public string Country { get; set; }
    public string Capital { get; set; }
    public string Population { get; set; }
}
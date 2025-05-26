using System.Text;
using Azure;
using FunctionCalls;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
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

var questionKernel = builder.Build();

questionKernel.FunctionInvocationFilters.Add(new LoggingFilter());
questionKernel.AutoFunctionInvocationFilters.Add(new EarlyTerminationFilter());


//await kernel.ImportPluginFromOpenApi("weatherForecast", new Uri("http//localhost"));

var executionSettings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
};

var chatCompletionService = questionKernel.GetRequiredService<IChatCompletionService>();

var chat = new ChatHistory("You are a usefull assistent");


var mathKernelbuilder = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(model, client);

mathKernelbuilder.Plugins.AddFromType<MathPlugin>();

var mathKernel = mathKernelbuilder.Build();
var mathChatCompletionService = questionKernel.GetRequiredService<IChatCompletionService>();

var mathchat = new ChatHistory("You are a usefull assistent");


KernelPluginFactory.CreateFromFunctions("AgentPLugin", [ AgentKernelFunctionFactory.CreateFromAgent(questionKernel)  ]);


while (true)
{
    Console.WriteLine("You: ");
    var prompt = Console.ReadLine();

    if (prompt == null)
    {
        break;
    }

    chat.AddUserMessage(prompt);

    var response = await chatCompletionService.GetChatMessageContentAsync(chat, executionSettings, questionKernel);

    Console.WriteLine($"AI: {response.Content}");
    chat.AddAssistantMessage(response.Content);
    Console.WriteLine();
}

public sealed class LoggingFilter : IFunctionInvocationFilter
{
    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        Console.WriteLine("FunctionInvoking - {0}.{1}", context.Function.PluginName, context.Function.Name);

        await next(context);

        Console.WriteLine("FunctionInvoked - {0}.{1}", context.Function.PluginName, context.Function.Name);
    }
}

public sealed class EarlyTerminationFilter : IAutoFunctionInvocationFilter
{
    public async Task OnAutoFunctionInvocationAsync(AutoFunctionInvocationContext context, Func<AutoFunctionInvocationContext, Task> next)
    {
        // Call the function first.
        await next(context);

        // Get a function result from context.
        var result = context.Result.GetValue<string>();

        // If the result meets the condition, terminate the process.
        // Otherwise, the function calling process will continue.
        if (result == "desired result")
        {
            context.Terminate = true;
        }
    }
}
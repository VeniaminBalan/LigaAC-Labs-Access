using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenApiClient.Models;
using System.Text;

namespace OpenApiClient.Services
{
    public class ChatService
    {
        private readonly IChatCompletionService _chatCompletionService;

        public ChatService(IChatCompletionService chatCompletionService)
        {
            _chatCompletionService = chatCompletionService;
        }

        public static double CalculateProbability(double probLog) => Math.Round(Math.Exp(probLog) * 100, 2);

        public async Task RunChatLoopAsync(string systemMessage = "You are a expert in Geography")
        {
            var chat = new ChatHistory(systemMessage);

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
                await foreach (var response in _chatCompletionService.GetStreamingChatMessageContentsAsync(chat, promptSettings2))
                {
                    Console.Write(response);
                    stringBuilder.Append(response);
                    
                    await Task.Delay(100);
                }
                
                chat.AddAssistantMessage(stringBuilder.ToString());
                Console.WriteLine();
            }
        }
    }
}

using Azure;
using Microsoft.Extensions.Configuration;
using OpenAI;

namespace OpenApiClient.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetApiKey()
        {
            return _configuration["VectorSearchEngineOptions:GithubPAT"] 
                ?? throw new ArgumentNullException("Github PAT is not set up");
        }

        public string GetModelId()
        {
            return _configuration["VectorSearchEngineOptions:ModelId"] 
                ?? throw new ArgumentNullException("Model Id is not set up");
        }

        public string GetEndpoint()
        {
            return _configuration["VectorSearchEngineOptions:Uri"] 
                ?? throw new ArgumentNullException("Uri is not set up");
        }

        public OpenAIClientOptions GetOpenAIClientOptions()
        {
            return new OpenAIClientOptions
            {
                Endpoint = new Uri(GetEndpoint()),
            };
        }
    }
}

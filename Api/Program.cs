using System.ClientModel;
using Api.Controllers.VectorSearch;
using Api.Models;
using Api.Models.VectorSearch;
using Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QuizContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped<QuestionsRepository>();
builder.Services.AddScoped<AnswersRepository>();


var searchOptions = builder.Configuration.GetSection(nameof(VectorSearchEngineOptions));
// Bind VectorSearchEngineOptions to configuration
builder.Services.Configure<VectorSearchEngineOptions>(searchOptions);


var uri = builder.Configuration["VectorSearchEngineOptions:Uri"];
var modelId = builder.Configuration["VectorSearchEngineOptions:ModelId"];
var githubPat = builder.Configuration["VectorSearchEngineOptions:GithubPAT"];

var client = new OpenAIClient(new ApiKeyCredential(githubPat), new OpenAIClientOptions { Endpoint = new Uri(uri) });

#pragma warning disable SKEXP0010
builder.Services.AddKernel()
    .AddOpenAITextEmbeddingGeneration(modelId, client)
#pragma warning restore SKEXP0010
    .AddRedisVectorStore("localhost:6379");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

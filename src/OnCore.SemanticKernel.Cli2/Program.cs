using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.Redis;
using OnCore.SemanticKernel.Cli2;
using OnCore.SemanticKernel.Cli2.Configuration;
using OnCore.SemanticKernel.Cli2.Middleware;
using OnCore.SemanticKernel.Cli2.Services;

#pragma warning disable SKEXP0020
#pragma warning disable SKEXP0010

var serviceCollection = new ServiceCollection();

serviceCollection.LoadConfiguration();

serviceCollection.AddChatClient(builder => builder
    // .UseLogging()
    .UseTrainingContentFilter()
    // .UseLanguage("Nederlands")
    .UseFunctionInvocation()
    .Use(AzureOpenAiChatClientFactory.Create("gpt-4o")
    // .Use(OllamaChatClientFactory.Create("phi4")
    // .Use(OllamaChatClientFactory.Create("deepseek-r1")
    // .Use(OllamaChatClientFactory.Create("llama3.2")
    ));

serviceCollection.AddScoped<IEmbeddingGenerator<string, Embedding<float>>>(_ => OllamaTextEmbeddingGeneratorServiceFactory.Create());

serviceCollection.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));

serviceCollection.AddScoped<RedisVectorStore>(x => RedisVectorStoreFactory.Create());

serviceCollection.AddScoped<ExerciseService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

// await new Step1_CreateAndUploadEmbeddingsToRedis().Run(serviceProvider);
    
// await new Step2_SearchEmbeddingsUsingRedis().Run(serviceProvider);

// await new Step3_LLM_Middleware().Run(serviceProvider);

// await new Step4_LLM_Functions().Run(serviceProvider);
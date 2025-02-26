using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OnCore.SemanticKernel.Cli.Configuration;

var serviceCollection = new ServiceCollection();

serviceCollection.LoadConfiguration().RegisterKernel();

var serviceProvider = serviceCollection.BuildServiceProvider();

var kernel = serviceProvider.GetRequiredService<Kernel>();

var chat = kernel.GetRequiredService<IChatCompletionService>();

// Enable planning
OpenAIPromptExecutionSettings openAiPromptExecutionSettings = new() 
{
    ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
    Temperature = 1,
    MaxTokens = 4096,
};

PromptExecutionSettings promptExecutionSettings = new()
{
#pragma warning disable SKEXP0001
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
#pragma warning restore SKEXP0001
};

var history = new ChatHistory();
history.AddUserMessage("What lights are currently on?");

// Get the response from the AI
var result = await chat.GetChatMessageContentsAsync(
    history,
    executionSettings: promptExecutionSettings,
    kernel: kernel);

// Print the results
foreach (var r in result)
{
    Console.WriteLine(r.Content);
}

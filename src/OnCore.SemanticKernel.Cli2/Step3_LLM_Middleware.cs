using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace OnCore.SemanticKernel.Cli2;

// ReSharper disable once InconsistentNaming
public class Step3_LLM_Middleware
{
    public async Task Run(IServiceProvider serviceProvider)
    {
        var chatClient  = serviceProvider.GetRequiredService<IChatClient>();
        
        var response = await chatClient.CompleteAsync("Hey, What AI modal are you? ");
        
        Console.WriteLine("[PT-Bot]: " + response.Message.Text);
    }
}
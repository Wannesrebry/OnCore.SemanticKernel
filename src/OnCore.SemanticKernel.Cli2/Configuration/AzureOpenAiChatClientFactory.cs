using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

namespace OnCore.SemanticKernel.Cli2.Configuration;

public class AzureOpenAiChatClientFactory
{
    public static IChatClient Create(string modal = "gpt-4o")
    {
        return new AzureOpenAIClient(
            new Uri(""),
            new AzureKeyCredential(
                "")
        ).AsChatClient(modal);
    }
}
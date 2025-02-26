using Microsoft.Extensions.AI;

namespace OnCore.SemanticKernel.Cli2.Configuration;

public static class OllamaChatClientFactory
{
    public static OllamaChatClient Create(string modal = "llama3.2")
    {
        return new OllamaChatClient(new Uri("http://127.0.0.1:11434"), modelId: modal);
    }
}
using Microsoft.Extensions.AI;

namespace OnCore.SemanticKernel.Cli2.Configuration;

public static class OllamaTextEmbeddingGeneratorServiceFactory
{
    public static IEmbeddingGenerator<string, Embedding<float>> Create(string modal = "nomic-embed-text")
    {
        return new OllamaEmbeddingGenerator("http://127.0.0.1:11434", modal);
    }
}
using Microsoft.SemanticKernel.Connectors.Redis;
using StackExchange.Redis;

#pragma warning disable SKEXP0020

namespace OnCore.SemanticKernel.Cli2.Configuration;

public static class RedisVectorStoreFactory
{
    public static RedisVectorStore Create()
    {
        return new RedisVectorStore(ConnectionMultiplexer.Connect("localhost:6379").GetDatabase());
    }
}
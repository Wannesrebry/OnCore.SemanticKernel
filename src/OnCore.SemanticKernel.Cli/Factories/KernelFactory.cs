using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnCore.SemanticKernel.Cli.Configuration;
using Serilog;

namespace OnCore.SemanticKernel.Cli.Factories;

internal class KernelFactory
{
    private readonly AzureOpenAiOptions _azureOpenAiOptions;
    private readonly SeqOptions _seqOptions;
    public KernelFactory(IOptions<AzureOpenAiOptions> azureOpenAiOptions, IOptions<SeqOptions> seqOptions)
    {
        _seqOptions = seqOptions.Value;
        _azureOpenAiOptions = azureOpenAiOptions.Value;
    }     
    
    public Kernel CreateKernel()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();
        
        AddLogger(builder);
        AddAzureOpenAiChatCompletion(builder);
        
        return builder.Build();
    }

    private void AddLogger(IKernelBuilder builder)
    {
        builder.Services.AddLogging(loggingBuilder =>
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(_seqOptions.Uri, apiKey: _seqOptions.ApiKey)
                .CreateLogger();

            loggingBuilder.AddSerilog(logger, dispose: true);
        });
    }
    
    private void AddAzureOpenAiChatCompletion(IKernelBuilder builder)
    {
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: _azureOpenAiOptions.DeploymentName,
            endpoint: _azureOpenAiOptions.Endpoint,
            apiKey: _azureOpenAiOptions.ApiKey
        );
    }
}
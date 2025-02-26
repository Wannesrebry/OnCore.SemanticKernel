using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OnCore.SemanticKernel.Cli2.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection LoadConfiguration(this IServiceCollection serviceCollection)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>() // Corrected method name
            .Build();
        
        serviceCollection.AddSingleton<IConfiguration>(configuration);

        serviceCollection.Configure<AzureOpenAiOptions>(configuration.GetSection("AzureOpenAi"));
        serviceCollection.Configure<SeqOptions>(configuration.GetSection("Seq"));
        return serviceCollection;
    }
}

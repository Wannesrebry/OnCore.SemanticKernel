using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnCore.SemanticKernel.Cli.Factories;

namespace OnCore.SemanticKernel.Cli.Configuration;

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
    
    public static IServiceCollection RegisterKernel(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<KernelFactory>();

        serviceCollection.AddSingleton<Kernel>(sp =>
        {
            var factory = sp.GetRequiredService<KernelFactory>();
            return factory.CreateKernel();
        });
        
        return serviceCollection;
    }
}
using Microsoft.Extensions.DependencyInjection;
using OnCore.SemanticKernel.Cli.Configuration;

var serviceCollection = new ServiceCollection();

serviceCollection
    .LoadConfiguration()
    .RegisterKernel();

var serviceProvider = serviceCollection.BuildServiceProvider();

var kernel = serviceProvider.GetRequiredService<Kernel>();

var result = await kernel.InvokePromptAsync("Give me a list of breakfast foods with eggs and cheese");
Console.WriteLine(result);

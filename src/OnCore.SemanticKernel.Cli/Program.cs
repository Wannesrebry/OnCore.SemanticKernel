using Microsoft.Extensions.DependencyInjection;
using OnCore.SemanticKernel.Cli.Configuration;
using OnCore.SemanticKernel.Cli.Functions;

var serviceCollection = new ServiceCollection();

serviceCollection.LoadConfiguration().RegisterKernel();

var serviceProvider = serviceCollection.BuildServiceProvider();

var kernel = serviceProvider.GetRequiredService<Kernel>();

var result = await kernel.InvokePromptAsync(
    "Give me a list of breakfast foods with eggs and cheese"
);
Console.WriteLine(result);

var result2 = await kernel.InvokePromptAsync("Hello, this is a test!");
Console.WriteLine(result2);

var testFunction = new ExampleKernelFunction();

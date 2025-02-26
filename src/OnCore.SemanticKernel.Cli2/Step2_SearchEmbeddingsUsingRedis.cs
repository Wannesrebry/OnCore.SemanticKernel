using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Redis;
using OnCore.SemanticKernel.Cli2.Configuration;
using OnCore.SemanticKernel.Cli2.Domain;
using OnCore.SemanticKernel.Cli2.Services;

#pragma warning disable SKEXP0020
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010

namespace OnCore.SemanticKernel.Cli2;

// ReSharper disable once InconsistentNaming
public class Step2_SearchEmbeddingsUsingRedis {

    public async Task Run(IServiceProvider serviceProvider)
    {
        ExerciseService exerciseService = serviceProvider.GetRequiredService<ExerciseService>();
        
        string searchQuery = "chin over bar";
        
        while (true)
        {
            Console.WriteLine("[PT-Bot]: Semantic search a exercise");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("[PT-Bot]: Invalid input, try again!");
                continue;
            }
            var exercise = await exerciseService.GetExerciseUsingSemanticSearch(input);
            Console.WriteLine("[PT-Bot]: " + exercise.FirstOrDefault().Name.ToString());
        }
    }
}
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OnCore.SemanticKernel.Cli2.Services;

namespace OnCore.SemanticKernel.Cli2;

// ReSharper disable once InconsistentNaming
public class Step4_LLM_Functions
{
    public async Task Run(IServiceProvider serviceProvider)
    {
        var chatClient  = serviceProvider.GetRequiredService<IChatClient>();
        var exerciseService = serviceProvider.GetRequiredService<ExerciseService>();
        
        var searchExerciseSemanticlyFunction = AIFunctionFactory.Create(exerciseService.GetExerciseUsingSemanticSearch);
        var exerciseDescriptionByKeyFunction = AIFunctionFactory.Create(exerciseService.GetExerciseDescriptionByKey);
        var saveWorkoutFunction = AIFunctionFactory.Create(exerciseService.SaveWorkout);

        var chatOptions = new ChatOptions()
        {
            Tools = [
                searchExerciseSemanticlyFunction,
                exerciseDescriptionByKeyFunction,
                saveWorkoutFunction
            ]
        };

        var chatMessages = new List<ChatMessage>()
        {
            new ChatMessage(ChatRole.System, 
                """
                You are a personal trainer. Use our exercise datastore to answer exercise-related questions.
                - Use `searchExerciseSemanticlyFunction` for semantic exercise searches.
                - Use `exerciseDescriptionByKeyFunction` for exercise instructions.
                - Use `saveWorkoutFunction` to save workouts.

                Before saving a workout, confirm with the user. Do not hallucinate; provide only our exercise data and instructions. Keep responses under 400 characters.
                """)
        };
        
        Console.WriteLine("[PT-Bot]: How can i help you on your fitness journey?");
        while (true)
        {
            Console.WriteLine("[You]: ");
            var input = Console.ReadLine();
            chatMessages.Add(new ChatMessage(ChatRole.User, input));
            
            var response = await chatClient.CompleteAsync(chatMessages, chatOptions);
            Console.WriteLine("[PT-Bot]: " + response.Message.Text);
        }
    }
}
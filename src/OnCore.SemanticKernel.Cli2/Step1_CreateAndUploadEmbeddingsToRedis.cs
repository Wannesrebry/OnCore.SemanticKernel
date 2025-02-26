using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.Redis;
using Microsoft.SemanticKernel.Embeddings;
using OnCore.SemanticKernel.Cli2.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.AI;
using OnCore.SemanticKernel.Cli2.Configuration;


#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0020

namespace OnCore.SemanticKernel.Cli2;

// ReSharper disable once InconsistentNaming
public class Step1_CreateAndUploadEmbeddingsToRedis
{
    public async Task Run(IServiceProvider serviceProvider)
    {
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = serviceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        var chatClient  = serviceProvider.GetRequiredService<IChatClient>();
        RedisVectorStore vectorStore = serviceProvider.GetRequiredService<RedisVectorStore>();

        var exercisesCollection = vectorStore.GetCollection<string, Exercise>(Constants.ExerciseCollectionName);
        await exercisesCollection.CreateCollectionIfNotExistsAsync();

        var exercises = GetExercisesFromCsv("./Exercises.csv").ToArray();

        await ProcessInBatchesAsync(exercises, 1, async exercise =>
        {
            try
            {
                if (await exercisesCollection.GetAsync(exercise.Key) is not null)
                {
                    return;
                }
                
                var description = await GetExerciseDescription(chatClient, exercise.ToString());
                exercise.Description = description;
                var embedding = await embeddingGenerator.GenerateEmbeddingAsync(exercise.ToString());
                exercise.Vector = embedding.Vector;
                await exercisesCollection.UpsertAsync(exercise);
            }
            catch(Exception ex){
                Console.WriteLine("[Error]" + ex.Message);
            }
        });
    }

    private async Task<string> GetExerciseDescription(IChatClient chatClient, string exerciseToDescribe)
    {
        var chatMessages = new List<ChatMessage>()
        {
            new ChatMessage(ChatRole.System, 
                """
                You are a personal trainer, responsible for documenting exercises. 
                Provide a clear and easy description of the provided exercise. 
                Keep it short, max 200 characters. 
                """),
            new ChatMessage(ChatRole.User, exerciseToDescribe)
        };
            
        var chatCompletion = await chatClient.CompleteAsync(chatMessages);
        return chatCompletion.Message.Text ?? string.Empty;
    }

    private Exercise[] GetExercisesFromCsv(string pathToCsvFile)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };

        using var reader = new StreamReader(pathToCsvFile);
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<ExerciseCsvRecord>().ToList();
        return records.Select(r => new Exercise
        {
            Key = r.Id,
            Name = r.Name,
            MuscleGroup = ((MuscleGroup)Enum.Parse(typeof(MuscleGroup), r.MuscleGroup)).ToString(),
            Type = ((ExerciseType)Enum.Parse(typeof(ExerciseType), r.Type)).ToString()
        }).ToArray();
    }
    
    private static async Task ProcessInBatchesAsync<T>(IEnumerable<T> items, int batchSize, Func<T, Task> processItem)
    {
        var itemList = items.ToList();

        for (int i = 0; i < itemList.Count; i += batchSize)
        {
            var batch = itemList.Skip(i).Take(batchSize);
            IEnumerable<Task> tasks = batch.Select(processItem);
            await Task.WhenAll(tasks);
            Console.WriteLine($"Processed batch {i / batchSize + 1} of {Math.Ceiling((double)itemList.Count / batchSize)}");
        }
    }
}

file class ExerciseCsvRecord
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string MuscleGroup { get; set; }
    public string Type { get; set; }
}
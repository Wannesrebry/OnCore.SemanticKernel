using System.ComponentModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Redis;
using OnCore.SemanticKernel.Cli2.Configuration;
using OnCore.SemanticKernel.Cli2.Domain;

#pragma warning disable SKEXP0020

namespace OnCore.SemanticKernel.Cli2.Services;

public class ExerciseService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _textEmbeddingGenerationService;
    private readonly RedisVectorStore _redisVectorStore;

    public ExerciseService(
        IEmbeddingGenerator<string, Embedding<float>> textEmbeddingGenerationService,
        RedisVectorStore redisVectorStore)
    {
        _textEmbeddingGenerationService = textEmbeddingGenerationService;
        _redisVectorStore = redisVectorStore;
    }
    
    [Description(
        """"
        Semanticly search exercises based on a searchQuery, muscleGroup can be used to filter on muscleGroup, amount specifies the amount of elements returned.
        Returns a array of 5 elemetns representing of closest correlating exercises.
        """")]
    public async Task<ExerciseDto[]> GetExerciseUsingSemanticSearch(string searchQuery, MuscleGroup? muscleGroup = null)
    {
        var collection = _redisVectorStore.GetCollection<string, Exercise>(Constants.ExerciseCollectionName);
        var searchVector = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(searchQuery);

        var vectorSearchOptions = new VectorSearchOptions()
        {
            Top = 5
        };

        if (muscleGroup is not null)
        {
            vectorSearchOptions = new VectorSearchOptions()
            {
                Top = 1,
                Filter = new VectorSearchFilter([
                    new EqualToFilterClause(nameof(Exercise.MuscleGroup), muscleGroup.ToString())
                ])
            };
        }
        
        var searchResults = await collection.VectorizedSearchAsync(searchVector.Vector, vectorSearchOptions);
        
        var searchResultItems = searchResults.Results.ToBlockingEnumerable().Select(x => x.Record).ToList();
        return searchResultItems.Select(x => new ExerciseDto()
        {
            Key = x.Key,
            MuscleGroup = x.MuscleGroup,
            Name = x.Name
        }).ToArray();
    }

    [Description(
        """"
        Get exercise description by Key
        """")]
    public async Task<string?> GetExerciseDescriptionByKey(string key)
    {
        var collection = _redisVectorStore.GetCollection<string, Exercise>(Constants.ExerciseCollectionName);
        var result = await collection.GetAsync(key);
        return result?.Description;
    }

    
    [Description(
        """"
        Save workout, provide a array of exercise keys
        """")]
    public async Task SaveWorkout(string[] exerciseKeys)
    {
        Console.Write("[Workout saved] - Trust me bro, it's saved, Exercises: ");
        foreach (string exerciseKey in exerciseKeys)
        {
            Console.Write(exerciseKey + ',');
        }
    }
}
using Microsoft.Extensions.VectorData;

namespace OnCore.SemanticKernel.Cli2.Domain;

public class Exercise
{
    [VectorStoreRecordKey]
    public string Key { get; set; }
    
    [VectorStoreRecordData]
    public string Name { get; set; }
    
    [VectorStoreRecordData]
    public string Description { get; set; }
    
    [VectorStoreRecordData(IsFilterable = true)]
    public string MuscleGroup { get; set; }
    
    [VectorStoreRecordData(IsFilterable = true)]
    public string Type { get; set; }
    
    [VectorStoreRecordVector(Dimensions: 768)]
    public ReadOnlyMemory<float> Vector { get; set; }
    
    public override string ToString()
    {
        var s =  $"Name: {Name} | MuscleGroup: {MuscleGroup} | Type: {Type}";
        if (!string.IsNullOrWhiteSpace(Description)) s += $" | Description: {Description}";
        return s;
    }
}

public enum MuscleGroup
{
    Quads = 1,
    Hamstrings = 2,
    Calves = 3,
    Back = 4,
    Chest = 5,
    Shoulders = 6,
    Triceps = 7,
    Biceps = 8,
    Neck = 9,
    Forearms = 10,
    Abs = 11,
    Glutes = 12,
    Cardio = 13,
    Other = 14
}

public enum ExerciseType
{
    Assistance = 0,
    Compound = 1,
    Remedial = 2
}

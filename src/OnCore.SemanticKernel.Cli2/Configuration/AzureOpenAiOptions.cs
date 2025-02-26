using System.ComponentModel.DataAnnotations;

namespace OnCore.SemanticKernel.Cli2.Configuration;

internal sealed class AzureOpenAiOptions
{
    [Required]
    public string Endpoint { get; set; }
    [Required]
    public string ApiKey { get; set; }
    [Required]
    public string DeploymentName { get; set; }
}
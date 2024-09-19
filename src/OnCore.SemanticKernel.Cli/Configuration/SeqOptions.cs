using System.ComponentModel.DataAnnotations;

namespace OnCore.SemanticKernel.Cli.Configuration;

public class SeqOptions
{
    [Required]
    public required string Uri { get; set; }
    public required string ApiKey { get; set; }
}

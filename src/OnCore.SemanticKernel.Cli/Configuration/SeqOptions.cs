using System.ComponentModel.DataAnnotations;

namespace OnCore.SemanticKernel.Cli.Configuration;

public class SeqOptions
{
    [Required]
    public string Uri { get; set; }
    public string ApiKey { get; set; }
}
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace BakedEnv.CLI.CommandArgs;

[Command("echo")]
public class Echo
{
    [Argument(0, "Text")]
    [Required]
    public string Text { get; set; }
    
    public int OnExecute(CommandLineApplication app)
    {
        Console.WriteLine(Text);

        return 0;
    }
}
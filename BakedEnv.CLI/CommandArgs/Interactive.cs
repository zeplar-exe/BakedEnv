using McMaster.Extensions.CommandLineUtils;

namespace BakedEnv.CLI.CommandArgs;

[Command("interactive")]
public class Interactive
{
    [Option("-e --exit", CommandOptionType.SingleValue)]
    public string? ExitMethod { get; set; }
        
    [Option("-s --silent", CommandOptionType.NoValue)]
    public bool Silent { get; set; }
    
    public int OnExecute(CommandLineApplication app)
    {
        using var session = new InteractiveSession(ExitMethod, Silent);
        
        return session.Run();
    }
}
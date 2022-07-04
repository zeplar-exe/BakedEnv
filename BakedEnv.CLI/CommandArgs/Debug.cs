using BakedEnv.Interpreter.Sources;
using McMaster.Extensions.CommandLineUtils;

namespace BakedEnv.CLI.CommandArgs;

[Command("debug")]
public class Debug
{
    [Option("-i --interactive")]
    public bool Interactive { get; set; }
        
    [Option("-f --file <path>", CommandOptionType.SingleValue)]
    public string? FilePath { get; set; }
        
    [Option("-r --raw <text>", CommandOptionType.SingleValue)]
    public string? RawString { get; set; }

    public int OnExecute(CommandLineApplication app)
    {
        var source = new StackSource();
    
        if (FilePath != null)
        {
            var fullFilePath = FilePath;

            if (!Path.IsPathRooted(FilePath))
                fullFilePath = Path.Join(Directory.GetCurrentDirectory(), FilePath);

            source.Push(File.ReadAllText(fullFilePath));
        }
        else if (RawString != null)
        {
            source.Push(RawString ?? string.Empty);
        }
    
        using var session = new DebugSession(source);

        return session.Run();
    }
}
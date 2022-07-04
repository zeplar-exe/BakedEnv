using System.Reflection;
using System.Text;
using BakedEnv.Interpreter.Sources;
using McMaster.Extensions.CommandLineUtils;

namespace BakedEnv.CLI.CommandArgs;

[Command("execute")]
public class Execute
{
    [Option("-f --file <path>", CommandOptionType.SingleValue)]
    [FileExists]
    public string? FilePath { get; set; }
        
    [Option("-r --raw <text>", CommandOptionType.SingleValue)]
    public string? RawString { get; set; }

    [Option("-s --silent", CommandOptionType.NoValue)]
    public bool Silent { get; set; }
    
    public int OnExecute(CommandLineApplication app)
    {
        IBakedSource source;
    
        if (FilePath != null)
        {
            var fullFilePath = FilePath;

            if (!Path.IsPathRooted(FilePath))
                fullFilePath = Path.Join(Directory.GetCurrentDirectory(), FilePath);

            source = new FileSource(fullFilePath, Encoding.Default);
        }
        else
        {
            source = new RawStringSource(RawString ?? string.Empty);
        }
    
        try
        {
            var session = new BakedEnvironment().CreateSession(source).Init();
            var result = session.ExecuteUntilTermination();

            if (!Silent)
            {
                Console.WriteLine(result);
            }
        }
        catch (Exception e)
        {
            var executable = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var logPath = Path.Join(executable.DirectoryName, "error.log");
            using var logFile = File.CreateText(logPath);
        
            if (!Silent)
            {
                var errorBuilder = new StringBuilder()
                    .AppendLine("Something went wrong:")
                    .AppendLine($"An exception of type '{e.GetType().Name}' occured.")
                    .AppendLine("The full error log can be found in the executable directory: ")
                    .Append(logPath)
                    .AppendLine("This is most likely a bug and should be reported on GitHub ")
                    .Append("(https://github.com/zeplar-exe/BakedEnv/issues).");

                Console.WriteLine(errorBuilder.ToString());
            }
        
            logFile.Write(e.ToString());

            return 1;
        }
    
        return 0;
    }
}
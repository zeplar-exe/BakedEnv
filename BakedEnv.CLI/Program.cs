using System.Text;
using BakedEnv;
using BakedEnv.CLI;
using BakedEnv.Interpreter.Sources;
using CommandLine;

var parserResult = Parser.Default.ParseArguments<CommandArgs, CommandArgs.ExecuteArgs>(args);

return parserResult.MapResult(
    (CommandArgs options) => ParseMainArgs(options),
    (CommandArgs.ExecuteArgs options) => ParseExecuteArgs(options),
    errors => 1);

int ParseMainArgs(CommandArgs mainArgs)
{
    return 0;
}

int ParseExecuteArgs(CommandArgs.ExecuteArgs executeArgs)
{
    IBakedSource source;
    
    if (executeArgs.FilePath != null)
    {
        var fullFilePath = executeArgs.FilePath!;

        if (!Path.IsPathRooted(executeArgs.FilePath))
            fullFilePath = Path.Join(Directory.GetCurrentDirectory(), executeArgs.FilePath);

        source = new FileSource(fullFilePath, Encoding.Default);
    }
    else
    {
        source = new RawStringSource(executeArgs.RawString ?? string.Empty);
    }

    if (executeArgs.Interactive)
    {
        using var session = new InteractiveSession();
        
        session.Start();
    }
    else if (executeArgs.Debug)
    {
        using var session = new DebugSession(source);

        session.Start();
    }
    else
    {
        var session = new BakedEnvironment().CreateSession(source).Init();
        var result = session.ExecuteUntilTermination();

        Console.WriteLine(result);
    }

    return 0;
}
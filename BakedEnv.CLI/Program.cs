using System.Reflection;
using System.Text;
using BakedEnv;
using BakedEnv.CLI;
using BakedEnv.Interpreter.Sources;
using CommandLine;

var parserResult = Parser.Default.ParseArguments<
    CommandArgs, 
    CommandArgs.ExecuteArgs, 
    CommandArgs.InteractiveArgs>
    (args);

parserResult.MapResult(
    (CommandArgs options) => ParseMainArgs(options),
    (CommandArgs.ExecuteArgs options) => ParseExecuteArgs(options),
    (CommandArgs.DebugArgs options) => ParseDebugArgs(options),
    (CommandArgs.InteractiveArgs options) => ParseInteractiveArgs(options),
    _ => 1);

int ParseMainArgs(CommandArgs mainArgs)
{
    return 0;
}

int ParseExecuteArgs(CommandArgs.ExecuteArgs executeArgs)
{
    IBakedSource source;
    
    if (executeArgs.FilePath != null)
    {
        var fullFilePath = executeArgs.FilePath;

        if (!Path.IsPathRooted(executeArgs.FilePath))
            fullFilePath = Path.Join(Directory.GetCurrentDirectory(), executeArgs.FilePath);

        source = new FileSource(fullFilePath, Encoding.Default);
    }
    else
    {
        source = new RawStringSource(executeArgs.RawString ?? string.Empty);
    }
    
    try
    {
        var session = new BakedEnvironment().CreateSession(source).Init();
        var result = session.ExecuteUntilTermination();

        if (!executeArgs.Silent)
        {
            Console.WriteLine(result);
        }
    }
    catch (Exception e)
    {
        var executable = new FileInfo(Assembly.GetExecutingAssembly().Location);
        var logPath = Path.Join(executable.DirectoryName, "error.log");
        using var logFile = File.CreateText(logPath);
        
        if (!executeArgs.Silent)
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

int ParseDebugArgs(CommandArgs.DebugArgs debugArgs)
{
    var source = new StackSource();
    
    if (debugArgs.FilePath != null)
    {
        var fullFilePath = debugArgs.FilePath;

        if (!Path.IsPathRooted(debugArgs.FilePath))
            fullFilePath = Path.Join(Directory.GetCurrentDirectory(), debugArgs.FilePath);

        source.Push(File.ReadAllText(fullFilePath));
    }
    else if (debugArgs.RawString != null)
    {
        source.Push(debugArgs.RawString ?? string.Empty);
    }
    
    using var session = new DebugSession(source);

    return session.Run();
}

int ParseInteractiveArgs(CommandArgs.InteractiveArgs interactiveArgs)
{
    using var session = new InteractiveSession(interactiveArgs);
        
    return session.Run();
}
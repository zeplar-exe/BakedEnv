using System.Text;
using BakedEnv.Extensions;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv.CLI;

public class InteractiveSession : IDisposable
{
    private const string LinePrefix = "> ";
    
    private StackSource Source { get; }
    private CommandArgs.InteractiveArgs Args { get; }
    
    public bool ExitRequested { get; set; }

    public InteractiveSession(CommandArgs.InteractiveArgs args)
    {
        Source = new StackSource();
        Args = args;
    }
    
    public int Run()
    {
        var environment = new BakedEnvironment()
            .WithReadOnlyBooleanVariables()
            .WithReadOnlyNullVariable();
        
        AddExitMethod(environment);

        var session = environment.CreateSession(Source).Init();
        
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        while (!ExitRequested)
        {
            Console.Write(LinePrefix);

            var line = Console.ReadLine();
            
            if (line == null)
                continue;
            
            Source.Push(line);

            var terminationValue = session.ExecuteUntilTermination();
            
            if (terminationValue == null)
                continue;
            
            if (!Args.Silent)
                Console.WriteLine(terminationValue.ToString());
        }

        return 0;
    }

    private void AddExitMethod(BakedEnvironment targetEnvironment)
    {
        if (Args.ExitMethod == null) 
            return;
        
        var exit = new BakedMethod(Enumerable.Empty<string>())
        {
            Instructions =
            {
                new ActionInstruction((_, _) => ExitRequested = true)
            }
        };

        targetEnvironment.GlobalVariables.Add(Args.ExitMethod, exit);
    }
    
    public void Dispose()
    {
        
    }
}
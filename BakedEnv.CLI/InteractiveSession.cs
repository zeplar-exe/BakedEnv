using BakedEnv.Extensions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv.CLI;

public class InteractiveSession : IDisposable
{
    private const string LinePrefix = "> ";
    
    private StackSource Source { get; }
    
    public string? ExitMethod { get; }
    public bool Silent { get; }
    
    public bool ExitRequested { get; set; }

    public InteractiveSession(string? exitMethod, bool silent)
    {
        Source = new StackSource();
        ExitMethod = exitMethod;
        Silent = silent;
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
            
            if (!Silent)
                Console.WriteLine(terminationValue.ToString());
        }

        return 0;
    }

    private void AddExitMethod(BakedEnvironment targetEnvironment)
    {
        if (ExitMethod == null) 
            return;

        var exit = new DelegateObject(delegate() { ExitRequested = true; });

        targetEnvironment.GlobalVariables.Add(ExitMethod, exit);
    }
    
    public void Dispose()
    {
        
    }
}
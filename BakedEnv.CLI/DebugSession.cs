using BakedEnv.Interpreter.Sources;

namespace BakedEnv.CLI;

public class DebugSession : IDisposable
{
    private int InstructionCount { get; }
    private TimeSpan TotalExecutionTime { get; }
    
    private IBakedSource Source { get; }

    public DebugSession(IBakedSource source)
    {
        Source = source;
    }

    public int Run()
    {
        return 0;
    }
    
    public void Dispose()
    {
        
    }
}
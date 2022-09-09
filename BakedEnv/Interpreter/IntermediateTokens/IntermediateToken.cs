using System.Runtime.CompilerServices;

namespace BakedEnv.Interpreter.IntermediateTokens;

public abstract class IntermediateToken
{
    public bool IsComplete { get; set; }

    public abstract ulong StartIndex { get; }
    public abstract int Length { get; }
    public abstract ulong EndIndex { get; }

    protected void AssertComplete([CallerMemberName] string caller = "missing")
    {
        if (IsComplete)
            return;
        
        throw new InvalidOperationException($"The caller, '{caller}' requires that this '({GetType().Name})' be complete.");
    }
}
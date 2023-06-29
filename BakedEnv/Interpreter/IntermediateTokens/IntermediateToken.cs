using System.Runtime.CompilerServices;

namespace BakedEnv.Interpreter.IntermediateTokens;

public abstract class IntermediateToken : ILowLevelToken
{
    public bool IsComplete { get; set; }

    public abstract long StartIndex { get; }
    public abstract long Length { get; }
    public abstract long EndIndex { get; }

    protected void AssertComplete([CallerMemberName] string caller = "missing")
    {
        if (IsComplete)
            return;
        
        throw new InvalidOperationException($"The caller, '{caller}' requires that this '({GetType().Name})' be complete.");
    }

    public abstract override string ToString();
}
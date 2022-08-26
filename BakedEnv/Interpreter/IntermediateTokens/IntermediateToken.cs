namespace BakedEnv.Interpreter.IntermediateTokens;

public abstract class IntermediateToken
{
    public bool IsComplete { get; set; }

    public abstract ulong StartIndex { get; }
    public abstract int Length { get; }
    public abstract ulong EndIndex { get; }
}
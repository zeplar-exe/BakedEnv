namespace BakedEnv.Interpreter.IntermediateTokens;

public abstract class IntermediateToken
{
    public bool IsComplete { get; private set; }
    

    public abstract int StartIndex { get; }
    public abstract int Length { get; }
    public abstract int EndIndex { get; }

    public IntermediateToken AsComplete(bool complete = true)
    {
        IsComplete = complete;
        
        return this;
    }
}
namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public abstract class PureIntermediateToken : IntermediateToken
{
    public abstract IEnumerable<IntermediateToken> ChildTokens { get; }
    
    public override ulong StartIndex => ChildTokens.FirstOrDefault()?.StartIndex ?? 0;
    public override int Length => ChildTokens.Sum(t => t.Length);
    public override ulong EndIndex => StartIndex + (ulong)Length;
}
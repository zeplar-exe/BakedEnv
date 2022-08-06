namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public abstract class PureIntermediateToken : IntermediateToken
{
    public abstract IEnumerable<IntermediateToken> ChildTokens { get; }
    
    public override int StartIndex => ChildTokens.FirstOrDefault()?.StartIndex ?? -1;
    public override int Length => ChildTokens.Sum(t => t.Length);
    public override int EndIndex => StartIndex + Length;
}
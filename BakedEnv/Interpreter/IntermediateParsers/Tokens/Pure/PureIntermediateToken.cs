namespace BakedEnv.Interpreter.IntermediateParsers.Tokens.Pure;

public class PureIntermediateToken : IntermediateToken
{
    public List<IntermediateToken> ChildTokens { get; }
    
    public override int StartIndex => ChildTokens.FirstOrDefault()?.StartIndex ?? -1;
    public override int Length => ChildTokens.Sum(t => t.Length);
    public override int EndIndex => StartIndex + Length;

    public PureIntermediateToken()
    {
        ChildTokens = new List<IntermediateToken>();
    }
    
    public T CopyTo<T>() where T : PureIntermediateToken, new()
    {
        var copy = new T();
        copy.ChildTokens.AddRange(ChildTokens);

        return copy;
    }
}
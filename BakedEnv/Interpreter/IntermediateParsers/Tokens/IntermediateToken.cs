using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens;

public class IntermediateToken
{
    public bool IsComplete { get; private set; }
    public List<IntermediateToken> ChildTokens { get; }

    public IntermediateToken()
    {
        ChildTokens = new List<IntermediateToken>();
    }

    public int StartIndex => ChildTokens.FirstOrDefault()?.StartIndex ?? -1;
    public int Length => ChildTokens.Sum(t => t.Length);
    public int EndIndex => StartIndex + Length;

    public T CopyTo<T>() where T : IntermediateToken, new()
    {
        var copy =  new T();
        copy.ChildTokens.AddRange(ChildTokens);

        return copy;
    }

    public IntermediateToken AsComplete(bool complete = true)
    {
        IsComplete = complete;
        
        return this;
    }
}
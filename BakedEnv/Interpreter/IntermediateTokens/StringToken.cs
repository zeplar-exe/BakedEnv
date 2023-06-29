namespace BakedEnv.Interpreter.IntermediateTokens;

public class StringToken : IntermediateToken
{
    public ILowLevelToken? Open { get; set; }
    public List<ILowLevelToken> Content { get; }
    public ILowLevelToken? Close { get; set; }

    public override long StartIndex => Open!.StartIndex;
    public override long Length => Open!.Length + Content.Sum(t => t.Length) + Close!.Length;
    public override long EndIndex => Close!.StartIndex;

    public StringToken()
    {
        Content = new List<ILowLevelToken>();
    }
    
    public override string ToString()
    {
        return Open + string.Concat(Content) + Close;
    }
}
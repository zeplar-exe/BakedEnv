namespace BakedEnv.Interpreter.IntermediateTokens;

public class MultiLineCommentToken : IntermediateToken
{
    public ILowLevelToken? Start { get; set; }
    public List<ILowLevelToken> Content { get; }
    public ILowLevelToken? End { get; set; }

    public override long StartIndex => Start!.StartIndex;
    public override long Length => Start!.Length + Content.Sum(t => t.Length) + End!.Length;
    public override long EndIndex => End!.EndIndex;

    public MultiLineCommentToken()
    {
        Content = new List<ILowLevelToken>();
    }
    
    public override string ToString()
    {
        return Start + string.Concat(Content) + End;
    }
}
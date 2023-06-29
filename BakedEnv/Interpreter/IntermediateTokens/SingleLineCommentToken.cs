namespace BakedEnv.Interpreter.IntermediateTokens;

public class SingleLineCommentToken : IntermediateToken
{
    public ILowLevelToken? StartToken { get; set; }
    public List<ILowLevelToken> Content { get; }

    public override long StartIndex => StartToken!.StartIndex;
    public override long Length => StartToken!.Length + Content.Sum(t => t.Length);
    public override long EndIndex => Content.LastOrDefault()?.EndIndex ?? StartToken!.EndIndex;

    public SingleLineCommentToken()
    {
        Content = new List<ILowLevelToken>();
    }

    public override string ToString()
    {
        return StartToken + string.Concat(Content);
    }
}
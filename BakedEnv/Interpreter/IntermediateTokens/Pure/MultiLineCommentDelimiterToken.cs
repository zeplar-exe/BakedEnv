using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class MultiLineCommentDelimiterToken : PureIntermediateToken
{
    public HashToken? FirstHash { get; set; }
    public HashToken? SecondHash { get; set; }

    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            if (FirstHash != null) yield return FirstHash;
            if (SecondHash != null) yield return SecondHash;
        }
    }
}
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class MultiLineCommentToken : PureIntermediateToken
{
    public MultiLineCommentDelimiterToken? Start { get; set; }
    public List<AnyToken> Content { get; }
    public MultiLineCommentDelimiterToken? End { get; set; }

    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            if (Start != null) yield return Start;

            foreach (var content in Content)
            {
                yield return content;
            }
            
            if (End != null) yield return End;
        }
    }

    public MultiLineCommentToken()
    {
        Content = new List<AnyToken>();
    }
}
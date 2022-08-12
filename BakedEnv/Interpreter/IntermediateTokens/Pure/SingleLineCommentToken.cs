using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class SingleLineCommentToken : PureIntermediateToken
{
    public HashToken? StartToken { get; set; }
    public List<AnyToken> Content { get; }

    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            if (StartToken != null) yield return StartToken;
            
            foreach (var content in Content)
            {
                yield return content;
            }
        }
    }

    public SingleLineCommentToken()
    {
        Content = new List<AnyToken>();
    }
}
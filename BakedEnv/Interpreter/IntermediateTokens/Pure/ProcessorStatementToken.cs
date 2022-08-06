using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class ProcessorStatementToken : PureIntermediateToken
{
    public LeftBracketToken? LeftBracket { get; set; }
    public RightBracketToken? RightBracket { get; set; }

    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            if (LeftBracket != null) yield return LeftBracket;
            if (RightBracket != null) yield return RightBracket;
        }
    }
}
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class ProcessorStatementToken : PureIntermediateToken
{
    public LeftBracketToken? LeftBracket { get; set; }
    public RightBracketToken? RightBracket { get; set; }
}
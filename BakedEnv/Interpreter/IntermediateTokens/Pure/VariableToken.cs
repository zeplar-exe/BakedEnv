using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.IntermediateTokens.Pure;

public class VariableToken : PureIntermediateToken
{
    public IdentifierToken Identifier { get; set; }
    
    public override IEnumerable<IntermediateToken> ChildTokens
    {
        get
        {
            yield return Identifier;
        }
    }
}
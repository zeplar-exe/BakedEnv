using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Identifiers;

public class IdentifierContinuationNode : InterpreterParser
{
    private IdentifierToken IdentifierToken { get; }

    public IdentifierContinuationNode(IdentifierToken identifierToken)
    {
        IdentifierToken = identifierToken;
    }

    public override DescendResult Descend(IntermediateToken token)
    {
        if (token is PeriodToken period)
        {
            
        }
        else if (token is EqualsToken equals)
        {
            
        }

        return DescendResult.Failure();
    }
}
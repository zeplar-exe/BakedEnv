using BakedEnv.Interpreter.IntermediateParsers.Tokens.Raw;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Interpreter.Variables;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens.Pure;

public class VariableToken : PureIntermediateToken
{
    public List<IdentifierToken> Identifiers { get; }
    public List<PeriodToken> Separators { get; }

    public VariableToken()
    {
        Identifiers = new List<IdentifierToken>();
        Separators = new List<PeriodToken>();
    }

    public VariableReference CreateReference(InvocationContext context)
    {
        return new VariableReference(Identifiers.Select(i => i.ToString()), context);
    }
}
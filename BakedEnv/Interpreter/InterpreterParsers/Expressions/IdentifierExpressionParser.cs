using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Variables;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class IdentifierExpressionParser : SingleExpressionParser
{
    public override bool AllowStartToken(IntermediateToken token)
    {
        return token is IdentifierToken;
    }

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var reference = new VariableReference(first.ToString(), context.Interpreter, context.Scope);

        return new VariableExpression(reference);
    }
}
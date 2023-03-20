using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Variables;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class IdentifierExpressionParser : SingleExpressionParser
{
    public override bool AllowToken(IntermediateToken token)
    {
        return token is IdentifierToken;
    }

    public override OperationResult<BakedExpression> Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var reference = new VariableReference(first.ToString(), context.Interpreter, context.Scope);

        return OperationResult<BakedExpression>.Success(new VariableExpression(reference));
    }
}
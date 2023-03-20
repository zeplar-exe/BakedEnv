using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ParenthesisExpressionParser : SingleExpressionParser
{
    public override bool AllowToken(IntermediateToken token)
    {
        return token is LeftParenthesisToken;
    }

    public override OperationResult<BakedExpression> Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        if (!iterator.TryMoveNext(out var next))
        {
            return OperationResult<BakedExpression>.Failure(BakedError.EEarlyEndOfFile(first.EndIndex));
        }
        
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(next, iterator, context);

        if (expression.HasError)
        {
            return expression;
        }

        if (!iterator.TryTakeNextOfType<RightParenthesisToken>(out _, out var error))
        {
            return OperationResult<BakedExpression>.Failure(error.Value);
        }

        return expression;
    }
}
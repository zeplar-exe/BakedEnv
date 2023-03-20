using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class FullExpressionParser
{
    public OperationResult<BakedExpression> Parse(IntermediateToken next, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(next, iterator, context);

        if (expression.HasError)
        {
            return expression;
        }

        var expressionContinuation = new ExpressionContinuationParser();
        var continuationResult = expressionContinuation.Parse(expression.Value, iterator, context);

        return continuationResult;
    }
}
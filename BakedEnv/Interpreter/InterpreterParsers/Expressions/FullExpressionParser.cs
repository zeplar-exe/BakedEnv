using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class FullExpressionParser
{
    public BakedExpression Parse(IntermediateToken next, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(next, iterator, context);
        
        if (!iterator.TryPeekNext(out _)) // End of file? No need to continue
            return expression;

        var expressionContinuation = new ExpressionContinuationParser();
        var continuationResult = expressionContinuation.Parse(expression, iterator, context);

        return continuationResult;
    }
}
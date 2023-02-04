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

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context, 
        out BakedError? error)
    {
        if (!iterator.TryMoveNext(out var next))
        {
            error = BakedError.EEarlyEndOfFile(first.EndIndex);

            return new NullExpression();
        }
        
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(next, iterator, context, out error);

        if (error != null)
        {
            return expression;
        }

        if (!iterator.TryTakeNextOfType<RightParenthesisToken>(out _, out error))
        {
            return expression;
        }

        return expression;
    }
}
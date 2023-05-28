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

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        if (!iterator.TryMoveNext(out var next))
        {
            BakedError.EEarlyEndOfFile(first.EndIndex).Throw();
        }
        
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(next, iterator, context);

        if (!iterator.TryTakeNextOfType<RightParenthesisToken>(out _, out var error))
        {
            error.Throw();

            return new NullExpression();
        }

        return expression;
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class IntegerExpressionParser : SingleExpressionParser
{
    public override bool AllowStartToken(IntermediateToken token)
    {
        return token is IntegerToken;
    }

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var i = new BakedInteger(first.ToString());

        return new ValueExpression(i);
    }
}
using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class DecimalExpressionParser : SingleExpressionParser
{
    public override bool AllowToken(IntermediateToken token)
    {
        return token is DecimalToken;
    }

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var bakedDecimal = new BakedDecimal(first.ToString());

        return new ValueExpression(bakedDecimal);
    }
}
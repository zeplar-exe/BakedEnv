using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class StringExpressionParser : SingleExpressionParser
{
    public override bool AllowToken(IntermediateToken token)
    {
        return token is StringToken;
    }

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var str = new BakedString(first.ToString());
        
        return new ValueExpression(str);
    }
}
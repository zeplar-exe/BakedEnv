using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class StringExpressionParser : SingleExpressionParser
{
    public override bool AllowStartToken(IntermediateToken token)
    {
        return token is StringToken;
    }

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var token = (StringToken)first;
        var str = new BakedString(string.Concat(token.Content));
        
        return new ValueExpression(str);
    }
}
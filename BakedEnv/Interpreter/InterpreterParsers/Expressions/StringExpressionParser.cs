using BakedEnv.Common;
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

    public override OperationResult<BakedExpression> Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var token = (StringToken)first;
        var str = new BakedString(string.Concat(token.Content));
        
        return OperationResult<BakedExpression>.Success(new ValueExpression(str));
    }
}
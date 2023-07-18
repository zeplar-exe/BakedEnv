using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ParenthesisExpressionParser : SingleExpressionParser
{
    public override bool AllowStartToken(IntermediateToken token)
    {
        return token.IsRawType(TextualTokenType.LeftParenthesis);
    }

    public override BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var next = iterator.MoveNextOrThrow();
        
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(next!, iterator, context);

        if (!iterator.TryTakeNextRaw(TextualTokenType.RightParenthesis, out _, out var error))
            error.Throw();

        return expression;
    }
}
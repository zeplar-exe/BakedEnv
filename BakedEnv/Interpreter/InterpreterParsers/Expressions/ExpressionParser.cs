using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionParser
{
    public BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var selector = new ExpressionSelector(context.Interpreter.Environment);
        var parser = selector.SelectParser(first);

        if (parser == null)
        {
            BakedError.EUnknownExpression(first.GetType().Name, first.StartIndex).Throw();
        }

        return parser.Parse(first, iterator, context);
    }
}
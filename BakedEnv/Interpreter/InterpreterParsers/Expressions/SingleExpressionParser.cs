using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public abstract class SingleExpressionParser
{
    public abstract bool AllowToken(IntermediateToken token);

    public abstract BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context);
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionParser
{
    public BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context, 
        out BakedError? error)
    {
        error = null;
        
        var selector = new ExpressionSelector();
        var parser = selector.SelectParser(first);

        if (parser == null)
        {
            error = BakedError.EUnknownExpression(first.GetType().Name, first.StartIndex);
            
            return new NullExpression();
        }

        var expression = parser.Parse(first, iterator, context);

        if (iterator.TryMoveNext(out var next))
        {
            switch (next)
            {
                case LeftParenthesisToken:
                    var tupleParser = new TupleParser();
                    var parameters = tupleParser.Parse(next, iterator, context, out error);

                    if (error != null)
                        return new NullExpression();

                    return new InvocationExpression(expression, parameters);
                default:
                    iterator.Reserve();
                    break;
            }
        }

        return expression;
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionParser
{
    public BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context, 
        out BakedError? error)
    {
        error = null;
        
        var selector = new ExpressionSelector(context.Interpreter.Environment);
        var parser = selector.SelectParser(first);

        if (parser == null)
        {
            error = BakedError.EUnknownExpression(first.GetType().Name, first.StartIndex);
            
            return new NullExpression();
        }

        var expression = parser.Parse(first, iterator, context, out error);

        if (error != null)
            return expression;

        if (iterator.TryMoveNext(out var next))
        {
            switch (next)
            {
                case LeftParenthesisToken:
                    var tupleParser = new ExpressionListParser();
                    var parameters = tupleParser.Parse(iterator, context, out error);

                    if (error != null)
                        return new NullExpression();

                    return new InvocationExpression(expression, parameters);
                default:
                    iterator.Reserve();
                    error = BakedError.EIncompleteStatement(next.GetType().Name, next.StartIndex);
                    
                    break;
            }
        }

        return expression;
    }
}
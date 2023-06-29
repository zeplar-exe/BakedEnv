using BakedEnv.Interpreter.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionContinuationParser
{
    public BakedExpression Parse(BakedExpression initial, InterpreterIterator iterator, ParserContext context)
    {
        if (iterator.TryMoveNext(out var next))
        {
            switch (next)
            {
                case LeftParenthesisToken:
                {
                    var tupleParser = new ExpressionListParser();
                    var parameters = tupleParser.Parse(iterator, context);

                    return new InvocationExpression(initial, parameters);
                }
                // what about chained continuations?
                default:
                {
                    iterator.Reserve();
                    BakedError.EUnknownExpression(next.GetType().Name, next.StartIndex).Throw();  
                    
                    break;
                }
            }
        }
        
        BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex).Throw();

        return new NullExpression();
    }
}
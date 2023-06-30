using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionContinuationParser
{
    public BakedExpression Parse(BakedExpression initial, InterpreterIterator iterator, ParserContext context)
    {
        if (iterator.TryMoveNext(out var next))
        {
            if (next.IsRawType(TextualTokenType.LeftParenthesis))
            {
                var tupleParser = new ExpressionListParser();
                var parameters = tupleParser.Parse(iterator, context);

                return new InvocationExpression(initial, parameters);
            } // what about chained continuations?
            else
            {
                iterator.Reserve();
                BakedError.EUnknownExpression(next.GetType().Name, next.StartIndex).Throw();
            }
        }
        
        BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex).Throw();

        return new NullExpression();
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionContinuationParser
{
    public BakedExpression Parse(BakedExpression initial, InterpreterIterator iterator, ParserContext context)
    {
        var next = iterator.MoveNextOrThrow();
    
        if (next.IsRawType(TextualTokenType.LeftParenthesis))
        {
            var tupleParser = new ExpressionListParser();
            var parameters = tupleParser.Parse(iterator, context);

            return new InvocationExpression(initial, parameters);
        } // what about chained continuations?
        else
        {
            BakedError.EUnknownExpression(next.GetType().Name, next.StartIndex).Throw();
            
            return new NullExpression();
        }
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ExpressionParser
{
    public InterpreterInternals Internals { get; }

    public ExpressionParser(InterpreterInternals internals)
    {
        Internals = internals;
    }
    
    public TryResult TryParseExpression(out BakedExpression? expression)
    {
        expression = null;
        
        var valueParser = Internals.Interpreter.CreateValueParser();
        var valueParseResult = valueParser.TryParseValue(out var value);
                                
        if (!valueParseResult.Success)
        {
            return valueParseResult;
        }

        if (value is IBakedCallable callable)
        {
            if (!Internals.Iterator.TryMoveNext(out var next))
            {
                return Internals.ErrorReporter.EndOfFileResult(Internals.Iterator.Current);
            }
            
            if (next.Is(LexerTokenId.LeftParenthesis))
            {
                var paramsParser = Internals.Interpreter.CreateParameterParser();
                var paramsResult = paramsParser.TryParseParameterList(out var parameters);

                if (!valueParseResult.Success)
                {
                    return paramsResult;
                }

                expression = new InvocationExpression(callable, parameters);

                return new TryResult(true);
            }
        }

        expression = new ValueExpression(value);

        return new TryResult(true);
    }
}
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ExpressionParser
{
    public BakedInterpreter Interpreter { get; }
    public InterpreterIterator Iterator { get; }

    public ExpressionParser(BakedInterpreter interpreter, InterpreterIterator iterator)
    {
        Interpreter = interpreter;
        Iterator = iterator;
    }
    
    public TryResult TryParseExpression(out BakedExpression? expression)
    {
        expression = null;
        
        var valueParser = Interpreter.CreateValueParser();
        var valueParseResult = valueParser.TryParseValue(out var value);
                                
        if (!valueParseResult.Success)
        {
            return valueParseResult;
        }

        if (value is IBakedCallable callable)
        {
            if (Iterator.Current.Is(LexerTokenId.LeftParenthesis))
            {
                var paramsParser = Interpreter.CreateParameterParser();
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
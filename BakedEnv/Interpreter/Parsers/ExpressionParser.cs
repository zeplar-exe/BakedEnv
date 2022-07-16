using BakedEnv.Interpreter.Expressions;

namespace BakedEnv.Interpreter.Parsers;

internal class ExpressionParser
{
    public ParserEnvironment Internals { get; }

    public ExpressionParser(ParserEnvironment internals)
    {
        Internals = internals;
    }

    public TryResult TryParseExpression(out BakedExpression expression)
    {
        return TryParseExpression(null, out expression);
    }
    
    private TryResult TryParseExpression(BakedExpression? previous, out BakedExpression expression)
    {
        expression = new NullExpression();

        if (Internals.TestEndOfFile(out var first, out var exitResult))
        {
            if (previous != null)
            {
                expression = previous;

                return new TryResult(true);
            }
            
            return exitResult;
        }

        switch (first.Id)
        {
            case LexerTokenId.Alphabetic:
            case LexerTokenId.AlphaNumeric:
            {
                Internals.Iterator.PushCurrent();
                
                if (previous != null)
                {
                    expression = previous;
                    
                    return new TryResult(true);
                }

                var valueParser = Internals.Interpreter.CreateValueParser();
                var identifierResult = valueParser.TryParseIdentifier(out var path);

                if (!identifierResult.Success)
                {
                    return identifierResult;
                }

                var reference = valueParser.GetVariableReference(path);
                expression = new VariableExpression(reference);

                break;
            }
            case LexerTokenId.LeftParenthesis:
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();

                if (previous != null)
                {
                    var parameterParser = Internals.Interpreter.CreateParameterParser();
                    var parameterResult = parameterParser.TryParseParameterList(out var parameters);

                    if (!parameterResult.Success)
                    {
                        return parameterResult;
                    }

                    expression = new InvocationExpression(previous, parameters);
                    
                    break;
                }
                
                var expressionResult = TryParseExpression(out var e);

                if (!expressionResult.Success)
                {
                    return expressionResult;
                }

                expression = new ParenthesisExpression(e);

                break;
            }
            default:
            {
                Internals.Iterator.PushCurrent();
                
                if (previous != null)
                {
                    expression = previous;

                    return new TryResult(true);
                }

                var valueParser = Internals.Interpreter.CreateValueParser();
                var valueParseResult = valueParser.TryParseValue(out var value);
                                
                if (!valueParseResult.Success)
                {
                    return valueParseResult;
                }
                
                expression = new ValueExpression(value);

                break;
            }
        }
        
        var tailExpressionResult = TryParseExpression(expression, out expression);
                
        if (!tailExpressionResult.Success)
        {
            return tailExpressionResult;
        }
                
        return new TryResult(true);
    }
}
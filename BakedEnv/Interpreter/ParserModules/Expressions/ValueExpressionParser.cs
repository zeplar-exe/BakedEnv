using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.ParserModules.Values;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ValueExpressionParser : ParserModule
{
    public ValueExpressionParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ValueExpressionParserResult Parse()
    {
        var builder = new ValueExpressionParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.AlphaNumeric: // Variable
            case LexerTokenType.Underscore:
            {
                if (first.ToString() == FunctionValueParser.Keyword)
                {
                    var functionParser = new FunctionValueParser(Internals);
                    var functionResult = functionParser.Parse();

                    if (functionResult.IsDeclaration)
                    {
                        if (!functionResult.IsComplete)
                        {
                            return builder.BuildFailure();
                        }

                        var expression = new ValueExpression(functionResult.Function);
                        
                        return builder.BuildSuccess(expression);
                    }
                }

                Internals.Iterator.PushCurrent();
                
                var identifierParser = new ChainIdentifierParser(Internals);
                var result = identifierParser.Parse();
                
                builder.AddTokensFrom(result);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                var reference = result.CreateReference(Internals.Interpreter);

                return builder.BuildSuccess(new VariableExpression(reference));
            }
            case LexerTokenType.Numeric: // Integer/Decimal
            case LexerTokenType.SingleQuotation: // String
            case LexerTokenType.DoubleQuotation:
            {
                Internals.Iterator.PushCurrent();
                
                var valueParser = new Values.ValueParser(Internals);
                var result = valueParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new ValueExpression(result.Value));
            }
            case LexerTokenType.LeftParenthesis: // Parenthesis
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();
                
                var tailParser = new ExpressionParser(Internals);
                var result = tailParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }
                
                if (!Internals.Iterator.TryMoveNext(out var end))
                {
                    return builder.BuildFailure();
                }

                if (end.Type != LexerTokenType.RightParenthesis)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new ParenthesisExpression(result.Expression));
            }
            case LexerTokenType.Dash: // Unary negation
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();
                
                var tailParser = new ExpressionParser(Internals);
                var result = tailParser.Parse();

                builder.AddTokensFrom(result);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new NegateExpression(result.Expression));
            }
        }

        return builder.BuildFailure();
    }
}
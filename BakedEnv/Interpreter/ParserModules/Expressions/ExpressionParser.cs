using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.ParserModules.Values;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ExpressionParser : ParserModule
{
    public ExpressionParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ExpressionParserResult Parse()
    {
        var builder = new ExpressionParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.AlphaNumeric: // Variable
            case LexerTokenType.Underscore:
            {
                Internals.Iterator.PushCurrent();
                
                var identifierParser = new ChainIdentifierParser(Internals);
                var result = identifierParser.Parse();
                
                builder.AddTokensFrom(result);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                var reference = result.CreateReference(Internals.Interpreter);

                if (reference.Path.Count == 0)
                {
                    if (reference.Name == FunctionExpressionParser.Keyword)
                    {
                        var functionParser = new FunctionExpressionParser(Internals);
                        var functionResult = functionParser.Parse();

                        if (functionResult.IsDeclaration)
                        {
                            if (!functionResult.IsComplete)
                            {
                                return builder.BuildFailure();
                            }

                            return builder.BuildSuccess(functionResult.Function);
                        }
                    }
                }
                
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
                
                if (!result.IsSuccess)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new ValueExpression(result.Value));
            }
            case LexerTokenType.LeftParenthesis: // Parenthesis
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();
                
                var tailParser = new TailExpressionParser(Internals);
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
                
                var tailParser = new TailExpressionParser(Internals);
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
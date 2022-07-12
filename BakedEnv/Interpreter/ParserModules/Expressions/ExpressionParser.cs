using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.ParserModules.Values;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ExpressionParser : ParserModule
{
    public ExpressionParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ExpressionParserResult Parse()
    {
        var builder = new ExpressionParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.AlphaNumeric: // Variable
            case LexerTokenType.Underscore:
            {
                Internals.Iterator.PushCurrent();
                
                using var identifierParser = new ChainIdentifierParser(Internals);
                var result = identifierParser.Parse();
                
                builder.WithTokens(result.AllTokens);

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
                
                using var valueParser = new Values.ValueParser(Internals);
                var result = valueParser.Parse();

                builder.WithTokens(result.AllTokens);
                
                if (!result.IsSuccess)
                {
                    return builder.BuildFailure();
                }

                return builder.BuildSuccess(new ValueExpression(result.Value));
            }
            case LexerTokenType.LeftParenthesis: // Parenthesis
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();
                
                using var tailParser = new TailExpressionParser(Internals);
                var result = tailParser.Parse();

                builder.WithTokens(result.AllTokens);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }
                
                if (Internals.TestEndOfFile(out var end, out eofResult))
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
                
                using var tailParser = new TailExpressionParser(Internals);
                var result = tailParser.Parse();

                builder.WithTokens(result.AllTokens);
                
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
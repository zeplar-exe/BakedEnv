using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Expressions.Arithmetic;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class TailExpressionParser : ParserModule
{
    public TailExpressionParser(InterpreterInternals internals) : base(internals)
    {

    }

    public TailExpressionParserResult Parse(ArithmeticInclusionMode arithmeticInclusion = ArithmeticInclusionMode.Include)
    {
        var builder = new TailExpressionParserResult.Builder();

        var expressionParser = new ExpressionParser(Internals);
        var result = expressionParser.Parse();

        builder.WithBaseExpression(result);

        if (!result.IsComplete)
        {
            return builder.BuildFailure();
        }

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        var tail = ParseTail(result.Expression);

        builder.WithTokens(tail.AllTokens);

        if (!tail.IsComplete)
        {
            return builder.BuildFailure();
        }

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.BuildFailure();
        }
        
        Internals.Iterator.PushCurrent();

        var expression = tail.Expression;
        
        if (arithmeticInclusion == ArithmeticInclusionMode.Include && IsArithmetic(first))
        {
            var arithmeticParser = new ArithmeticParser(Internals);
            var arithmeticResult = arithmeticParser.ParseFrom(tail);

            if (!arithmeticResult.IsComplete)
            {
                expression = arithmeticResult.CreateExpression();
            }
        }
        
        return builder.BuildSuccess(expression);
    }

    private TailExpressionParserResult ParseTail(BakedExpression previous)
    {
        BakedExpression? newExpression;

        var builder = new TailExpressionParserResult.Builder();

        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.BuildFailure();
        }
        
        Internals.Iterator.PushCurrent();

        switch (first.Type)
        {
            case LexerTokenType.LeftParenthesis: // Invocation
            {
                var parameterParser = new ArgumentListParser(Internals);
                var result = parameterParser.Parse();

                builder.WithTokens(result.AllTokens);

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                var parameters = result.Expressions.Expressions.Select(p => p.Expression).ToArray();

                newExpression = new InvocationExpression(previous, parameters);

                break;
            }
            case LexerTokenType.LeftBracket: // Indexer
            {
                var indexerParser = new IndexerParser(Internals);
                var result = indexerParser.Parse();

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                newExpression = new IndexExpression(previous, result.Expression.Expression);

                break;
            }
            default:
            {
                return builder.BuildSuccess(previous);
            }
        }

        var tailResult = ParseTail(newExpression);

        builder.WithTokens(tailResult.AllTokens);

        if (!tailResult.IsComplete)
        {
            return builder.BuildFailure();
        }

        return builder.BuildSuccess(tailResult.Expression);
    }
    
    private static bool IsArithmetic(LexerToken token)
    {
        return token.Type is
            LexerTokenType.Plus or
            LexerTokenType.Dash or
            LexerTokenType.Star or
            LexerTokenType.Slash or
            LexerTokenType.Caret or
            LexerTokenType.Percent;
    }
}

public enum ArithmeticInclusionMode
{
    Include,
    Exclude
}
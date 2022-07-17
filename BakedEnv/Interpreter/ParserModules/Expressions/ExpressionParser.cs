using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.ParserModules.DataStructures;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ExpressionParser : ParserModule
{
    public ExpressionParser(ParserEnvironment internals) : base(internals)
    {

    }

    public ExpressionParserResult Parse(ArithmeticInclusionMode arithmeticInclusion = ArithmeticInclusionMode.Include)
    {
        var builder = new ExpressionParserResult.Builder();

        var valueParser = new ValueExpressionParser(Internals);
        var valueResult = valueParser.Parse();

        builder.WithBaseExpression(valueResult);

        if (!valueResult.IsComplete)
        {
            return builder.BuildFailure();
        }

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        var tail = ParseTail(valueResult.Expression);

        builder.AddTokensFrom(tail);

        if (!tail.IsComplete)
        {
            return builder.BuildFailure();
        }

        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (!Internals.Iterator.TryPeekNext(out var first))
        {
            return builder.BuildFailure();
        }

        var expression = tail.Expression;
        
        if (arithmeticInclusion == ArithmeticInclusionMode.Include && IsArithmetic(first))
        {
            var arithmeticParser = new ArithmeticParser(Internals);
            var arithmeticResult = arithmeticParser.ParseFrom(tail);

            if (!arithmeticResult.IsComplete)
            {
                return builder.BuildFailure();
            }
            
            expression = arithmeticResult.CreateExpression();
        }
        
        return builder.WithChain(tail.Chain).WithChainExpression(expression).BuildSuccess(expression);
    }

    private ExpressionParserResult ParseTail(BakedExpression previous)
    {
        BakedExpression newExpression;

        var builder = new ExpressionParserResult.Builder();

        if (!Internals.Iterator.TryPeekNext(out var first))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.LeftParenthesis: // Invocation
            {
                var parameterParser = new ArgumentListParser(Internals);
                var result = parameterParser.Parse();

                builder.AddTokensFrom(result);

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
                var indexerParser = new ArrayParser(Internals);
                var result = indexerParser.Parse();

                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                newExpression = new IndexExpression(previous, 
                    result
                        .ExpressionList
                        .Expressions
                        .Select(e => e.Expression));

                break;
            }
            default:
            {
                return builder.BuildSuccess(previous);
            }
        }

        builder.WithChainExpression(newExpression);

        var tailResult = ParseTail(newExpression);

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
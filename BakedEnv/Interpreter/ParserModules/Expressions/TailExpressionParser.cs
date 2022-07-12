using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class TailExpressionParser : ParserModule
{
    public TailExpressionParser(InterpreterInternals internals) : base(internals)
    {
        
    }
    
    public TailExpressionParserResult Parse()
    {
        var builder = new TailExpressionParserResult.Builder();

        using var expressionParser = new ExpressionParser(Internals);
        var result = expressionParser.Parse();

        builder.WithTokens(result.AllTokens);
        
        if (!result.IsComplete)
        {
            return builder.BuildFailure();
        }

        var tail = ParseTail(result.Expression);
        
        builder.WithTokens(tail.AllTokens);
        
        if (!tail.IsComplete)
        {
            return builder.BuildFailure();
        }

        return builder.BuildSuccess(tail.Expression);
    }
    
    private ExpressionParserResult ParseTail(BakedExpression previous)
    {
        BakedExpression newExpression;
        
        var builder = new ExpressionParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.BuildFailure();
        }
        
        Internals.Iterator.PushCurrent();

        switch (first.Type)
        {
            case LexerTokenType.LeftParenthesis: // Invocation
            {
                var parameterParser = new ParameterListParser(Internals);
                var result = parameterParser.Parse();

                builder.WithTokens(result.AllTokens);
                
                if (!result.IsComplete)
                {
                    return builder.BuildFailure();
                }

                break;
            }
            case LexerTokenType.LeftBracket: // Indexer
            {
                break;
            }
            case LexerTokenType.Plus: // Addition
            {
                break;
            }
            case LexerTokenType.Dash: // Subtraction
            {
                break;
            }
            case LexerTokenType.Star: // Multiplication
            {
                break;
            }
            case LexerTokenType.Slash: // Division
            {
                break;
            }
            case LexerTokenType.Caret: // Exponentiation
            {
                break;
            }
            case LexerTokenType.Percent: // Modulus
            {
                break;
            }
            default:
            {
                return builder.BuildSuccess(previous);
            }
        }

        var tailResult = ParseTail(previous);

        if (!tailResult.IsComplete)
        {
            return builder.BuildFailure();
        }

        builder.WithTokens(tailResult.AllTokens);

        return builder.BuildSuccess(tailResult.Expression);
    }
}
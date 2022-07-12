using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ArgumentListParser : ParserModule
{
    public ArgumentListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ParameterListParserResult Parse()
    {
        var builder = new ParameterListParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftParenthesis)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        var expectValue = true;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.RightParenthesis:
                {
                    return builder.WithClosing(token).Build(!expectValue);
                }
                case LexerTokenType.Comma:
                {
                    if (expectValue)
                    {
                        return builder.Build(false);
                    }
                    
                    expectValue = true;
                    
                    break;
                }
                default:
                {
                    Internals.Iterator.PushCurrent();

                    var expressionParser = new TailExpressionParser(Internals);
                    var result = expressionParser.Parse();
                    
                    builder.WithTailExpression(result);

                    if (!result.IsComplete)
                    {
                        return builder.Build(false);
                    }
                    
                    expectValue = false;
                    
                    break;
                }
            }
            
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
        }

        return builder.Build(false);
    }
}
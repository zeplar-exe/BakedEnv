using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class StringParser : ParserModule
{
    public StringParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public StringParserResult Parse()
    {
        var builder = new StringParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.Build(false);
        }

        if (first.Type is not LexerTokenType.SingleQuotation or LexerTokenType.DoubleQuotation)
        {
            return builder.Build(false);
        }

        builder.WithQuotation(first);

        var escaped = false;

        LexerToken? previous = null;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.SingleQuotation when first.Type == LexerTokenType.SingleQuotation:
                case LexerTokenType.DoubleQuotation when first.Type == LexerTokenType.DoubleQuotation:
                {
                    if (escaped)
                    {
                        escaped = false;
                        builder.WithContentToken(token);
                        break;
                    }
                    
                    return builder.WithQuotation(token).Build(true);
                }
                case LexerTokenType.Backslash:
                {
                    escaped = true;
                    break;
                }
                default:
                {
                    if (escaped)
                    {
                        builder.WithContentToken(previous!);
                    }
                    
                    escaped = false;
                    builder.WithContentToken(token);
                    
                    break;
                }
            }

            previous = token;
        }

        return builder.Build(false);
    }
}
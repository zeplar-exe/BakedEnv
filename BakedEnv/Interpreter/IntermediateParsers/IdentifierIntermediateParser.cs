using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class IdentifierIntermediateParser : MatchIntermediateParser
{
    public override bool Match(TextualToken first)
    {
        return first.Type is TextualTokenType.Underscore or TextualTokenType.Alphabetic;
    }

    public override IntermediateToken Parse(TextualToken first, LexerIterator iterator)
    {
        var identifier = new IdentifierToken();

        identifier.Tokens.Add(first);

        var validTokens = new[] { TextualTokenType.Alphabetic, TextualTokenType.Underscore, TextualTokenType.Numeric };

        while (!iterator.Ended)
        {
            if (iterator.NextIs(validTokens, out var next))
            {
                identifier.Tokens.Add(next);
            }
            else
            {
                iterator.Reserve();
                break;
            }
        }

        return identifier.AsComplete();
    }
}
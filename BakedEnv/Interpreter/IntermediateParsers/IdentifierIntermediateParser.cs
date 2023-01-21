using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class IdentifierIntermediateParser : MatchIntermediateParser
{
    public override bool Match(TextualToken first)
    {
        return IdentifierToken.InitialTokenTypes.Contains(first.Type);
    }

    public override IntermediateToken Parse(TextualToken first, LexerIterator iterator)
    {
        var identifier = new IdentifierToken();

        identifier.RawTokens.Add(first);

        while (!iterator.Ended)
        {
            if (iterator.NextIsAny(IdentifierToken.TokenTypes, out var next))
            {
                identifier.RawTokens.Add(next);
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
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class IdentifierParser : MatchParser
{
    private static HashSet<TextualTokenType> IdentifierTokenTypes = new()
    {
        TextualTokenType.Alphabetic, 
        TextualTokenType.Underscore
    };
    
    public override bool Match(TextualToken first)
    {
        return IdentifierTokenTypes.Contains(first.Type);
    }

    public override IntermediateToken Parse(TextualToken first, ParserIterator iterator)
    {
        var identifier = new IdentifierToken();

        identifier.RawTokens.Add(first);

        while (!iterator.Ended)
        {
            if (iterator.NextIsAny(IdentifierTokenTypes, out var next))
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
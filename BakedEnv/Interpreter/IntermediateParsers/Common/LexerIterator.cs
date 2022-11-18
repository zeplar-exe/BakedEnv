using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public class LexerIterator : EnumerableIterator<TextualToken>
{
    public LexerIterator(IEnumerable<TextualToken> enumerable) : base(enumerable)
    {
        
    }
    
    public bool NextIs(TextualTokenType type, [NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        if (!SkipTrivia(out token))
            return false;

        if (token.Type != type)
            return false;

        return true;
    }
    
    public bool NextIsAny(IEnumerable<TextualTokenType> types, [NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        if (!SkipTrivia(out token))
            return false;

        var type = token.Type;

        if (types.Any(t => t == type))
            return false;

        return true;
    }
    
    public bool SkipTrivia([NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        while (true)
        {
            if (!TryMoveNext(out var next))
                return false;
            
            switch (next.Type)
            {
                case TextualTokenType.Space:
                case TextualTokenType.Tab:
                case TextualTokenType.LineFeed:
                case TextualTokenType.CarriageReturn:
                case TextualTokenType.CarriageReturnLineFeed:
                {
                    continue;
                }
                default:
                {
                    token = next;
                    break;
                }
            }
            
            break;
        }

        return true;
    }
}
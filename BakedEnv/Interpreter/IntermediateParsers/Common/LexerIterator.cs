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
    
    public bool NextIs(IEnumerable<TextualTokenType> types, [NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        if (!SkipTrivia(out token))
            return false;

        var type = token.Type;

        return types.Any(t => t == type);
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
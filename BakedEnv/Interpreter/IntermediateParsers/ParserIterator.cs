using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class ParserIterator : EnumerableIterator<LexerToken>
{
    public ParserIterator(IEnumerable<LexerToken> enumerable) : base(enumerable)
    {
        
    }
    
    public bool NextIs(LexerTokenType type, [NotNullWhen(true)] out LexerToken? token)
    {
        token = null;

        if (!SkipTrivia(out token))
            return false;

        if (token.Type != type)
            return false;

        return true;
    }
    
    public bool SkipTrivia([NotNullWhen(true)] out LexerToken? token)
    {
        token = null;

        while (true)
        {
            if (!TryMoveNext(out var next))
                return false;
            
            switch (next.Type)
            {
                case LexerTokenType.Space:
                case LexerTokenType.Tab:
                case LexerTokenType.LineFeed:
                case LexerTokenType.CarriageReturn:
                case LexerTokenType.CarriageReturnLineFeed:
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
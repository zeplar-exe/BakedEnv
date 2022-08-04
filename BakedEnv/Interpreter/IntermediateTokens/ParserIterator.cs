using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class ParserIterator : EnumerableIterator<LexerToken>
{
    public ParserIterator(IEnumerable<LexerToken> enumerable) : base(enumerable)
    {
        
    }

    public bool NextIs(LexerTokenType type, [NotNullWhen(true)] out LexerToken? token)
    {
        if (!TryMoveNext(out token))
            return false;

        if (token.Type != type)
            return false;

        return true;
    }
}
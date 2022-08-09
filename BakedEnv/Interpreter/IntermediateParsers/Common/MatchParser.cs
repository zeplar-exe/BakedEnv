using BakedEnv.Interpreter.IntermediateTokens;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public abstract class MatchParser
{
    public abstract bool Match(LexerToken first);
    public abstract IntermediateToken Parse(LexerToken first, ParserIterator iterator);
    
    protected bool TestTokenIs(LexerToken token, params LexerTokenType[] types)
    {
        return types.Any(t => token.Type == t);
    }
}
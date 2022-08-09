using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public abstract class MatchParser
{
    public abstract TryMatchResult TryParse(LexerToken first, ParserIterator iterator);
    
    protected bool TestTokenIs(LexerToken token, params LexerTokenType[] types)
    {
        return types.Any(t => token.Type == t);
    }
}
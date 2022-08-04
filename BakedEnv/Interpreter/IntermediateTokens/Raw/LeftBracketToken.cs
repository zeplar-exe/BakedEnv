using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class LeftBracketToken : RawIntermediateToken
{
    public LeftBracketToken(LexerToken token) : base(token, LexerTokenType.LeftBracket)
    {
        
    }
}
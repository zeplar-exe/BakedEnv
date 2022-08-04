using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class RightBracketToken : RawIntermediateToken
{
    public RightBracketToken(LexerToken token) : base(token, LexerTokenType.RightBracket)
    {
        
    }
}
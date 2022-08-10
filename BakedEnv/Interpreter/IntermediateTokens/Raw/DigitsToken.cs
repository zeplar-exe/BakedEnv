using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class DigitsToken : RawIntermediateToken
{
    public DigitsToken(LexerToken token) : base(token, LexerTokenType.Numeric)
    {
        
    }
}
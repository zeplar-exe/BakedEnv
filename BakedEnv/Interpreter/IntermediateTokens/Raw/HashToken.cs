using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class HashToken : RawIntermediateToken
{
    public HashToken(LexerToken token) : base(token, LexerTokenType.Hashtag)
    {
        
    }
}
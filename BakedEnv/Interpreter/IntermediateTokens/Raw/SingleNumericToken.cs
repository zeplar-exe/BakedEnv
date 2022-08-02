using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class SingleNumericToken : RawIntermediateToken
{
    public SingleNumericToken(LexerToken token) : base(token, LexerTokenType.Numeric)
    {
        
    }

    public ulong AsUlong() => ulong.Parse(RawToken.ToString());
}
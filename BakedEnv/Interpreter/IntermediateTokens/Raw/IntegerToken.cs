using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IntegerToken : RawIntermediateToken
{
    public IntegerToken(LexerToken token) : base(token, LexerTokenType.Numeric)
    {
        
    }
    
    public bool TryParse(out ulong i)
    {
        return ulong.TryParse(RawToken.ToString(), out i);
    }
}
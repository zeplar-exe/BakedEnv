using BakedEnv.Interpreter.IntermediateParsers.Tokens.Raw;
using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens.Single;

public class SingleNumericToken : RawIntermediateToken
{
    public SingleNumericToken(LexerToken token) : base(token, LexerTokenType.Numeric)
    {
        
    }

    public ulong AsUlong() => ulong.Parse(RawToken.ToString());
}
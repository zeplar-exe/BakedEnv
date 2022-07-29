using BakedEnv.Interpreter.IntermediateParsers.Tokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens.Single;

public class PeriodToken : RawIntermediateToken
{
    public PeriodToken(LexerToken token, LexerTokenType expected) : base(token, expected)
    {
        
    }

    public PeriodToken(LexerToken token, params LexerTokenType[] expected) : base(token, expected)
    {
        
    }
}
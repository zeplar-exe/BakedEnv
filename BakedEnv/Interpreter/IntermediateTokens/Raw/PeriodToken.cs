using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class PeriodToken : RawIntermediateToken
{
    public PeriodToken(TextualToken token, TextualTokenType expected) : base(token, expected)
    {
        
    }

    public PeriodToken(TextualToken token, params TextualTokenType[] expected) : base(token, expected)
    {
        
    }
}
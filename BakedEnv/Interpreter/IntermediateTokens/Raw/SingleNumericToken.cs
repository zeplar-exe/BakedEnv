using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class SingleNumericToken : RawIntermediateToken
{
    public SingleNumericToken(TextualToken token) : base(token, TextualTokenType.Numeric)
    {
        
    }

    public ulong AsUlong() => ulong.Parse(RawToken.ToString());
}
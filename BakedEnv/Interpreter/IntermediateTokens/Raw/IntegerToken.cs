using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IntegerToken : RawIntermediateToken
{
    public IntegerToken(TextualToken token) : base(token, TextualTokenType.Numeric)
    {
        
    }
    
    public bool TryParse(out ulong i)
    {
        return ulong.TryParse(RawToken.ToString(), out i);
    }
}
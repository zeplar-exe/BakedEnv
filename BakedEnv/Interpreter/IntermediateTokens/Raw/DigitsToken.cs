using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class DigitsToken : RawIntermediateToken
{
    public DigitsToken(TextualToken token) : base(token, TextualTokenType.Numeric)
    {
        
    }
}
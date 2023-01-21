using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class RightBracketToken : RawIntermediateToken
{
    public RightBracketToken(TextualToken token) : base(token, TextualTokenType.RightBracket)
    {
        
    }
}
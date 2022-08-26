using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class LeftBracketToken : RawIntermediateToken
{
    public LeftBracketToken(TextualToken token) : base(token, TextualTokenType.LeftBracket)
    {
        
    }
}
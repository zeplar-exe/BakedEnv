using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class RightCurlyBracketToken : RawIntermediateToken
{
    public RightCurlyBracketToken(TextualToken token) : base(token, TextualTokenType.RightCurlyBracket)
    {
        
    }
}
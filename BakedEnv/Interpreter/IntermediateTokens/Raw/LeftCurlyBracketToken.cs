using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class LeftCurlyBracketToken : RawIntermediateToken
{
    public LeftCurlyBracketToken(TextualToken token) : base(token, TextualTokenType.LeftCurlyBracket)
    {
        
    }
}
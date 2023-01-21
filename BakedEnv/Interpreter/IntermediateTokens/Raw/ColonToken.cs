using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class ColonToken : RawIntermediateToken
{
    public ColonToken(TextualToken token) : base(token, TextualTokenType.Colon)
    {
        
    }
}
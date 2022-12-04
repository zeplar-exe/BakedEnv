using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class EqualsToken : RawIntermediateToken
{
    public EqualsToken(TextualToken token) : base(token, TextualTokenType.Equals)
    {
        
    }
}
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class CommaToken : RawIntermediateToken
{
    public CommaToken(TextualToken token) : base(token, TextualTokenType.Comma)
    {
        
    }
}
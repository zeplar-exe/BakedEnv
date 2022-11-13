using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class AnyToken : RawIntermediateToken
{
    public AnyToken(TextualToken token) : base(token)
    {
        
    }
}
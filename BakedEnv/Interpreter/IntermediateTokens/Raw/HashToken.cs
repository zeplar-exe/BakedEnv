using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class HashToken : RawIntermediateToken
{
    public HashToken(TextualToken token) : base(token, TextualTokenType.Hashtag)
    {
        
    }
}
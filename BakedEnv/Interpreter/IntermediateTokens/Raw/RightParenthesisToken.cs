using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class RightParenthesisToken : RawIntermediateToken
{
    public RightParenthesisToken(TextualToken token) : base(token, TextualTokenType.RightParenthesis)
    {
        
    }
}
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class LeftParenthesisToken : RawIntermediateToken
{
    public LeftParenthesisToken(TextualToken token) : base(token, TextualTokenType.LeftParenthesis)
    {
        
    }
}
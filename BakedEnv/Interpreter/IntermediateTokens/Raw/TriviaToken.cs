using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class TriviaToken : RawIntermediateToken
{
    public TriviaToken(LexerToken token) : base(token, 
        LexerTokenType.Space, LexerTokenType.Tab, 
        LexerTokenType.LineFeed, LexerTokenType.CarriageReturn, LexerTokenType.CarriageReturnLineFeed)
    {
        
    }
}
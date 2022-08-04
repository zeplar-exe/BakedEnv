using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class RawIntermediateToken : IntermediateToken
{
    public GuardedLexerToken RawToken { get; }
    public LexerTokenType Type => RawToken.Get().Type;
    
    public override int StartIndex => RawToken.Get().StartIndex;
    public override int Length => RawToken.Get().Length;
    public override int EndIndex => StartIndex + Length;

    public RawIntermediateToken(LexerToken token, LexerTokenType expected)
    {
        RawToken = new GuardedLexerToken(token, expected);
    }

    public RawIntermediateToken(LexerToken token, params LexerTokenType[] expected)
    {
        RawToken = new GuardedLexerToken(token, expected);
    }
}
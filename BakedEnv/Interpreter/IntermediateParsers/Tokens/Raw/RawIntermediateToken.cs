using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens.Raw;

public class RawIntermediateToken : IntermediateToken
{
    public GuardedLexerToken RawToken { get; }
    
    public override int StartIndex => RawToken.Get().StartIndex;
    public override int Length => RawToken.Get().Length;
    
    public RawIntermediateToken(LexerToken token, LexerTokenType expected)
    {
        RawToken = new GuardedLexerToken(token, expected);
    }

    public RawIntermediateToken(LexerToken token, params LexerTokenType[] expected)
    {
        RawToken = new GuardedLexerToken(token, expected);
    }
}
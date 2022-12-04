using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public abstract class RawIntermediateToken : IntermediateToken
{
    public GuardedLexerToken RawToken { get; }
    public TextualTokenType Type => RawToken.Get().Type;
    
    public override ulong StartIndex => RawToken.Get().StartIndex;
    public override int Length => RawToken.Get().Length;
    public override ulong EndIndex => StartIndex + (ulong)Length;

    protected RawIntermediateToken(TextualToken token, TextualTokenType expected)
    {
        RawToken = new GuardedLexerToken(token, expected);
        IsComplete = true;
    }

    protected RawIntermediateToken(TextualToken token, params TextualTokenType[] expected)
    {
        RawToken = new GuardedLexerToken(token, expected);
        IsComplete = true;
    }

    public override string ToString()
    {
        return RawToken.ToString();
    }
}
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public abstract class RawIntermediateTokenGroup : IntermediateToken
{
    public GuardedLexerTokenList RawTokens { get; }

    public override ulong StartIndex => RawTokens.FirstOrDefault()?.StartIndex ?? 0;
    public override int Length => RawTokens.Sum(t => t.Length);
    public override ulong EndIndex => StartIndex + (ulong)Length;

    public RawIntermediateTokenGroup(TextualTokenType expected)
    {
        RawTokens = new GuardedLexerTokenList(expected);
        IsComplete = true;
    }
    
    public RawIntermediateTokenGroup(IEnumerable<TextualTokenType> expected)
    {
        RawTokens = new GuardedLexerTokenList(expected);
    }

    public override string ToString()
    {
        return string.Concat(RawTokens);
    }
}
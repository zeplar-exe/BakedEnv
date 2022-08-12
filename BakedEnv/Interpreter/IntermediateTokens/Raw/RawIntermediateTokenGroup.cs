using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public abstract class RawIntermediateTokenGroup : IntermediateToken
{
    public GuardedLexerTokenList RawTokens { get; }

    public override int StartIndex => RawTokens.FirstOrDefault()?.StartIndex ?? -1;
    public override int Length => RawTokens.Sum(t => t.Length);
    public override int EndIndex => StartIndex + Length;

    public RawIntermediateTokenGroup(LexerTokenType expected)
    {
        RawTokens = new GuardedLexerTokenList(expected);
        IsComplete = true;
    }

    public RawIntermediateTokenGroup(params LexerTokenType[] expected)
    {
        RawTokens = new GuardedLexerTokenList(expected);
    }

    public override string ToString()
    {
        return string.Concat(RawTokens);
    }
}
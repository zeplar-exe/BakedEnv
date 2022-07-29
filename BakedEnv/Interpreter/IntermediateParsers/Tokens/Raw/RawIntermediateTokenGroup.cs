using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens.Raw;

public class RawIntermediateTokenGroup : IntermediateToken
{
    public GuardedLexerTokenList RawTokens { get; }

    public override int StartIndex => RawTokens.FirstOrDefault()?.StartIndex ?? -1;
    public override int Length => RawTokens.Sum(t => t.Length);
    public override int EndIndex => StartIndex + Length;

    public RawIntermediateTokenGroup(LexerTokenType expected)
    {
        RawTokens = new GuardedLexerTokenList(expected);
    }

    public RawIntermediateTokenGroup(params LexerTokenType[] expected)
    {
        RawTokens = new GuardedLexerTokenList(expected);
    }
}
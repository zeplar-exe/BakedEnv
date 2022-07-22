using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens;

public record IntermediateToken(LexerToken[] Tokens, IntermediateTokenType Type)
{
    public int StartIndex => Tokens.FirstOrDefault()?.StartIndex ?? -1;
    public int Length => Tokens.Sum(t => t.Length);
    public int EndIndex => StartIndex + Length;
}
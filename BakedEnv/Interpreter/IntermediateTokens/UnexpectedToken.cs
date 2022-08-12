using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class UnexpectedToken : IntermediateToken
{
    public LexerToken Token { get; }

    public override int StartIndex => Token.StartIndex;
    public override int Length => Token.Length;
    public override int EndIndex => Token.EndIndex;

    public UnexpectedToken(LexerToken token)
    {
        Token = token;
    }
}
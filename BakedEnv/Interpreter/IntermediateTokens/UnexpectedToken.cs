using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens;

public class UnexpectedToken : IntermediateToken
{
    public TextualToken Token { get; }

    public override ulong StartIndex => Token.StartIndex;
    public override int Length => Token.Length;
    public override ulong EndIndex => Token.EndIndex;

    public UnexpectedToken(TextualToken token)
    {
        Token = token;
    }
}
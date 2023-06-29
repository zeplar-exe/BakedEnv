using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class RawIntermediateToken : IntermediateToken
{
    public TextualToken RawToken { get; }
    public TextualTokenType Type => RawToken.Type;
    
    public override long StartIndex => RawToken.StartIndex;
    public override long Length => RawToken.Length;
    public override long EndIndex => StartIndex + Length;

    public RawIntermediateToken(TextualToken token)
    {
        RawToken = token;
        IsComplete = true;
    }

    public override string ToString()
    {
        return RawToken.ToString();
    }
}
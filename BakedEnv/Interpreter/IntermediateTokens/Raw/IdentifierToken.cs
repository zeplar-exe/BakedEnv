using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IdentifierToken : RawIntermediateTokenGroup
{
    public IdentifierToken() : base(LexerTokenType.AlphaNumeric, LexerTokenType.Numeric, LexerTokenType.Underscore)
    {
        
    }

    public override string ToString()
    {
        return string.Join("", RawTokens);
    }
}
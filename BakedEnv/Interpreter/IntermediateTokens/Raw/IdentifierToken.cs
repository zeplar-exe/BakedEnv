using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IdentifierToken : RawIntermediateTokenGroup
{
    public IdentifierToken() : base(TextualTokenType.AlphaNumeric, TextualTokenType.Numeric, TextualTokenType.Underscore)
    {
        
    }

    public override string ToString()
    {
        return string.Join("", RawTokens);
    }
}
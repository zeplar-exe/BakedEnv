using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IdentifierToken : RawIntermediateTokenGroup
{
    public IdentifierToken() : base(TextualTokenType.Alphabetic, TextualTokenType.Numeric, TextualTokenType.Underscore)
    {
        
    }

    public override string ToString()
    {
        return string.Join("", RawTokens);
    }
}
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IdentifierToken : RawIntermediateTokenGroup
{
    public static readonly HashSet<TextualTokenType> InitialTokenTypes = new()
    {
        TextualTokenType.Alphabetic,
        TextualTokenType.Underscore,
    };

    public static readonly HashSet<TextualTokenType> TokenTypes = new()
    {
        TextualTokenType.Alphabetic,
        TextualTokenType.Underscore,
        TextualTokenType.Numeric
    };

    public IdentifierToken() : base(TokenTypes.ToArray())
    {
        
    }
}
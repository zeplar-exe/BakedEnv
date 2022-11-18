using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class IdentifierToken : RawIntermediateTokenGroup
{
    public static readonly HashSet<TextualTokenType> TokenTypes = new()
    {
        TextualTokenType.Alphabetic,
        TextualTokenType.Underscore
    };

    public IdentifierToken() : base(TokenTypes.ToArray())
    {
        
    }
}
using BakedEnv.Interpreter.IntermediateParsers.Common;
using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens;

public class IdentifierToken : IntermediateToken
{
    public LexerTokenList LexerTokens { get; }

    public IdentifierToken()
    {
        LexerTokens = new LexerTokenList(
            LexerTokenType.AlphaNumeric, LexerTokenType.Numeric, LexerTokenType.Underscore);
    }

    public override string ToString()
    {
        return string.Join("", LexerTokens);
    }
}
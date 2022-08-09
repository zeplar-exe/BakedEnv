using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public static class ParseAssertExtensions
{
    public static LexerToken AssertIsType(this LexerToken? token, LexerTokenType type)
    {
        if (token == null)
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be a non-null LexerToken.");
        }

        if (token.Type != type)
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be of type '{type}', got {token.Type}.");
        }

        return token;
    }
    
    public static LexerToken AssertIsType(this LexerToken? token, params LexerTokenType[] types)
    {
        if (token == null)
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be a non-null LexerToken.");
        }

        if (!types.Contains(token.Type))
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be one of the following types '{string.Join(',', types)}'," +
                $" got {token.Type}.");
        }

        return token;
    }
}
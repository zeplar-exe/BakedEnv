using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public static class ParseAssertExtensions
{
    public static TextualToken AssertIsType(this TextualToken? token, TextualTokenType type)
    {
        if (token == null)
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be a non-null TextualToken.");
        }

        if (token.Type != type)
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be of type '{type}', got {token.Type}.");
        }

        return token;
    }
    
    public static TextualToken AssertIsType(this TextualToken? token, params TextualTokenType[] types)
    {
        if (token == null)
        {
            throw new InvalidOperationException(
                $"Expected {nameof(token)} to be a non-null TextualToken.");
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
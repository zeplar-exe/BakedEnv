using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter;

public static class ErrorMessages
{
    public const string EndOfFile = "Unexpected end of file.";

    public const string ValueExpected = "Expected a value (string, number, variable).";
    
    public static string ExpectedTokenOfType(LexerToken token, params LexerTokenType[] expected)
    {
        var expectedText = string.Join(" OR ", expected);

        return $"Unexpected token. Expected {expectedText}, got '{token.Type}'.";
    }

    public static string InvalidOperation(string operationName, BakedObject value)
    {
        return $"Cannot {operationName} a value of type '{value.GetType().Name}'.";
    }
}
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
    
    public static string InvalidUnaryOperation(string operationName, BakedObject value)
    {
        return $"Cannot {operationName} a value of type '{value.GetType().Name}'.";
    }

    public static string InvalidBinaryOperation(string operationName, BakedObject left, BakedObject right)
    {
        return $"Cannot {operationName} a value of type '{right.GetType().Name}' from '{left.GetType().Name}'.";
    }

    public static string InvalidIndex(BakedObject target, IEnumerable<BakedObject> values)
    {
        return $"Cannot index a value of type '{target.GetType().Name}' with " +
               $"'{string.Join(", ", values.Select(v => v.GetType().Name))}'.";
    }
}
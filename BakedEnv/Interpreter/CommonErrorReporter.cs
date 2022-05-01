using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter;

internal class CommonErrorReporter
{
    private BakedInterpreter Interpreter { get; }

    public CommonErrorReporter(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    public bool TestUnexpectedTokenTypeError(LexerToken token, params LexerTokenId[] expected)
    {
        if (!expected.Any(token.Is))
        {
            ReportUnexpectedTokenType(token, expected);

            return true;
        }

        return false;
    }
    
    public void ReportUnexpectedTokenType(LexerToken token, params LexerTokenId[] expected)
    {
        var expectedText = string.Join(", ", expected);

        Interpreter.ReportError(
            null,
            $"Unexpected token. Expected enum '{expectedText}', got '{token.Id.ToString()}'.",
            token.Span.Start);
    }

    public void ReportInvalidValue(LexerToken token)
    {
        Interpreter.ReportError(
            null, 
            "Expected a value (string, number, variable).",
            token.Span.Start);
    }
}
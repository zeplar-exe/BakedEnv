using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Parsers;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter;

internal class CommonErrorReporter
{
    private BakedInterpreter Interpreter { get; }

    public CommonErrorReporter(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    public bool TestUnexpectedTokenType(LexerToken token, out BakedError error, params LexerTokenId[] expected)
    {
        error = default;
        
        if (!expected.Any(token.Is))
        {
            error = ReportUnexpectedTokenType(token, expected);

            return true;
        }

        return false;
    }
    
    public BakedError ReportUnexpectedTokenType(LexerToken token, params LexerTokenId[] expected)
    {
        var expectedText = string.Join(", ", expected);

        return Interpreter.ReportError(
            null,
            $"Unexpected token. Expected enum '{expectedText}', got '{token.Id.ToString()}'.",
            token.Span.Start);
    }

    public BakedError ReportInvalidValue(LexerToken token)
    {
        return Interpreter.ReportError(
            null, 
            "Expected a value (string, number, variable).",
            token.Span.Start);
    }
    
    public BakedError ReportEndOfFile(LexerToken token)
    {
        return Interpreter.ReportError(
            null,
            "Unexpected end of file.",
            token.Span.End);
    }
    
    public TryResult EndOfFileResult(LexerToken token)
    {
        return new TryResult(false, ReportEndOfFile(token));
    }
}
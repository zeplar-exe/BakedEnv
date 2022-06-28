using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Instructions;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter;

internal class CommonErrorReporter
{
    private BakedInterpreter Interpreter { get; }

    public CommonErrorReporter(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }

    public InvalidInstruction CreateInstruction(string? id, string message, int sourceIndex)
    {
        var error = Interpreter.ReportError(id, message, sourceIndex);

        return new InvalidInstruction(error);
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
            "Expected a value (string, number, variable, etc.).",
            token.Span.Start);
    }

    public BakedError ReportEndOfFile(LexerToken token)
    {
        return Interpreter.ReportError(
            null,
            "Unexpected end of file.",
            token.Span.End);
    }
}
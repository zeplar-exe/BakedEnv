using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Parsers;
using Jammo.ParserTools.Lexing;
using TokenCs;

namespace BakedEnv.Interpreter;

internal class CommonErrorReporter
{
    private BakedInterpreter Interpreter { get; }

    public CommonErrorReporter(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    public bool TestUnexpectedTokenType(LexerToken token, out BakedError error, params LexerTokenType[] expected)
    {
        error = default;
        
        if (expected.All(t => token.Type != t))
        {
            error = ReportUnexpectedTokenType(token, expected);

            return true;
        }

        return false;
    }
    
    public BakedError ReportUnexpectedTokenType(LexerToken token, params LexerTokenType[] expected)
    {
        return Interpreter.ReportError(
            ErrorCodes.InvalidTokenType,
            ErrorMessages.ExpectedTokenOfType(token, expected),
            token.Span.Start);
    }

    public BakedError ReportInvalidValue(LexerToken token)
    {
        return Interpreter.ReportError(
            ErrorCodes.InvalidValue,
            ErrorMessages.ValueExpected,
            token.Span.Start);
    }
    
    public BakedError ReportEndOfFile(LexerToken token)
    {
        return Interpreter.ReportError(
            ErrorCodes.EndOfFile,
            ErrorMessages.EndOfFile,
            token.Span.End);
    }
    
    public TryResult EndOfFileResult(LexerToken token)
    {
        return new TryResult(false, ReportEndOfFile(token));
    }
}
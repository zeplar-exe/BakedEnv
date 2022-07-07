using BakedEnv.Interpreter.Instructions;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ProcessorStatementParser
{
    private InterpreterInternals Internals { get; }
    
    public ProcessorStatementParser(InterpreterInternals internals)
    {
        Internals = internals;
    }

    public InterpreterInstruction Parse()
    {
        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (!Internals.Iterator.TryMoveNext(out var nameToken))
        {
            return new InvalidInstruction(Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
        }

        if (Internals.ErrorReporter.TestUnexpectedTokenType(nameToken, out var nameError,
                LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
        {
            return new InvalidInstruction(nameError);
        }

        var name = nameToken.ToString();
                
        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (!Internals.Iterator.TryMoveNext(out var colonToken))
        {
            return new InvalidInstruction(Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
        }
                
        if (Internals.ErrorReporter.TestUnexpectedTokenType(colonToken, out var colonError, 
                LexerTokenId.Colon))
        {
            return new InvalidInstruction(colonError);
        }
                
        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        var valueParser = Internals.Interpreter.CreateExpressionParser();
        var parseResult = valueParser.TryParseExpression(out var expression);
                
        if (!parseResult.Success)
        {
            return new InvalidInstruction(Internals.ErrorReporter.ReportInvalidValue(nameToken));
        }

        return new ProcessorStatementInstruction(name, expression, nameToken.Span.Start);
    }
}
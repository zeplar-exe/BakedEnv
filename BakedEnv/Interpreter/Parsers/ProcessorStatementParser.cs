using BakedEnv.Interpreter.Instructions;
using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter.Parsers;

internal class ProcessorStatementParser
{
    private BakedInterpreter Interpreter { get; }
    private InterpreterIterator Iterator { get; }
    private IteratorTools IteratorTools { get; }
    private CommonErrorReporter ErrorReporter { get; }
    
    public ProcessorStatementParser(BakedInterpreter interpreter, InterpreterIterator iterator, IteratorTools iteratorTools, CommonErrorReporter errorReporter)
    {
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        ErrorReporter = errorReporter;
    }

    public InterpreterInstruction Parse()
    {
        IteratorTools.SkipWhitespaceAndNewlines();
                
        var nameToken = Iterator.Current;

        if (ErrorReporter.TestUnexpectedTokenType(nameToken, out var nameError,
                LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
        {
            return new InvalidInstruction(nameError);
        }

        var name = nameToken.ToString();
                
        IteratorTools.SkipWhitespaceAndNewlines();
                
        var colonToken = Iterator.Current;
                
        if (ErrorReporter.TestUnexpectedTokenType(colonToken, out var colonError, 
                LexerTokenId.Colon))
        {
            return new InvalidInstruction(colonError);
        }
                
        IteratorTools.SkipWhitespaceAndNewlines();

        var valueParser = Interpreter.CreateValueParser();
        var parseResult = valueParser.TryParseValue(out var value);
                
        if (!parseResult.Success)
        {
            return new InvalidInstruction(ErrorReporter.ReportInvalidValue(nameToken));
        }

        return new ProcessorStatementInstruction(name, value, nameToken.Span.Start);
    }
}
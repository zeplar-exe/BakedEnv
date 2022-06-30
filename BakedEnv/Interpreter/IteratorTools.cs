using Jammo.ParserTools.Lexing;

namespace BakedEnv.Interpreter;

public class IteratorTools
{
    private BakedInterpreter Interpreter { get; }
    private InterpreterIterator Iterator { get; }
    
    public IteratorTools(BakedInterpreter interpreter, InterpreterIterator iterator)
    {
        Interpreter = interpreter;
        Iterator = iterator;
    }
    
    public int SkipWhitespace()
    {
        Interpreter.AssertReady();
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
    
    public int SkipWhitespaceAndNewlines()
    {
        Interpreter.AssertReady();
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
    
    public int SkipWhitespaceAndNewlinesReserved()
    {
        Interpreter.AssertReady();
        
        if (Iterator.Current.Id is not LexerTokenId.Whitespace or LexerTokenId.Newline)
            return 0;
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
}
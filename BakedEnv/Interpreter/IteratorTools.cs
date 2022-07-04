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
        
        if (stride == 0 || !Iterator.AtEnd)
            Iterator.PushCurrent();

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
        
        if (stride == 0 || !Iterator.AtEnd)
            Iterator.PushCurrent();

        return stride;
    }
    
    public int SkipWhitespaceAndNewlinesReserved()
    {
        Interpreter.AssertReady();
        
        var stride = 0;
        
        if (Iterator.Started)
        {
            if (Iterator.Current.Id is not LexerTokenId.Whitespace or LexerTokenId.Newline)
            {
                return 0;
            }
            else
            {
                stride += Iterator.Current.Span.Size;
            }
        }

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }
        
        if (stride == 0 || !Iterator.AtEnd)
            Iterator.PushCurrent();

        return stride;
    }
}
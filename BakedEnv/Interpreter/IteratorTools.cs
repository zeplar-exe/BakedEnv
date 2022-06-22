using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter;

public class IteratorTools
{
    private BakedInterpreter Interpreter { get; }
    private EnumerableIterator<LexerToken> Iterator { get; }
    
    public IteratorTools(BakedInterpreter interpreter, EnumerableIterator<LexerToken> iterator)
    {
        Interpreter = interpreter;
        Iterator = iterator;
    }
    
    private int SkipWhitespace()
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
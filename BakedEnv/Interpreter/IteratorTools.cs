using TokenCs;
using TokenCs.Extensions;

namespace BakedEnv.Interpreter;

internal class IteratorTools
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

        foreach (var token in Iterator.PeekTakeWhile(IsWhitespace))
        {
            stride += token.Length;
        }
        
        if (stride == 0 || !Iterator.Ended)
            Iterator.PushCurrent();

        return stride;
    }
    
    public int SkipWhitespaceAndNewlines()
    {
        Interpreter.AssertReady();
        
        var stride = 0;

        foreach (var token in Iterator.PeekTakeWhile(t => t.IsNewLine() || IsWhitespace(t)))
        {
            stride += token.Length;
        }
        
        if (stride == 0 || !Iterator.Ended)
            Iterator.PushCurrent();

        return stride;
    }
    
    public int SkipWhitespaceAndNewlinesReserved()
    {
        Interpreter.AssertReady();
        
        var stride = 0;
        
        if (Iterator.Started)
        {
            if (Iterator.Current.IsNewLine() || IsWhitespace(Iterator.Current))
            {
                return 0;
            }
            
            stride += Iterator.Current.Length;
        }

        foreach (var token in Iterator.PeekTakeWhile(t => t.IsNewLine() || IsWhitespace(t)))
        {
            stride += token.Length;
        }
        
        if (stride == 0 || !Iterator.Ended)
            Iterator.PushCurrent();

        return stride;
    }

    private bool IsWhitespace(LexerToken token)
    {
        return token.Type is LexerTokenType.Space or LexerTokenType.Tab;
    }
}
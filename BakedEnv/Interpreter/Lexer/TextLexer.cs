using System.Collections;

namespace BakedEnv.Interpreter.Lexer;

public class TextLexer : IEnumerable<TextualToken>, IDisposable
{
    private IEnumerator<char> Source { get; }

    public TextLexer(IEnumerable<char> source)
    {
        Source = source.GetEnumerator();
    }

    public IEnumerator<TextualToken> GetEnumerator()
    {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public void Dispose()
    {
        Source.Dispose();
    }
}
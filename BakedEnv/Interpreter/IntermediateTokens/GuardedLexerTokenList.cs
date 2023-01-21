using System.Collections;

using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens;

public class GuardedLexerTokenList : IList<TextualToken>
{
    private TextualTokenType[] Expected { get; }
    private List<GuardedLexerToken> Tokens { get; }

    public int Count => Tokens.Count;
    public bool IsReadOnly => false;

    public GuardedLexerTokenList(TextualTokenType expected)
    {
        Expected = new[] { expected };
        Tokens = new List<GuardedLexerToken>();
    }
    
    public GuardedLexerTokenList(IEnumerable<TextualTokenType> expected)
    {
        Expected = expected.ToArray();
        Tokens = new List<GuardedLexerToken>();
    }
    
    public TextualToken this[int index]
    {
        get => Tokens[index].Get();
        set => Tokens[index] = new GuardedLexerToken(value);
    }

    public void Add(TextualToken token)
    {
        Tokens.Add(new GuardedLexerToken(token, Expected));
    }

    public void Clear()
    {
        Tokens.Clear();
    }

    public bool Contains(TextualToken item)
    {
        return Tokens.Any(t => t.Get() == item);
    }

    public void CopyTo(TextualToken[] array, int arrayIndex)
    {
        Tokens.CopyTo(array.Select(t => new GuardedLexerToken(t, Expected)).ToArray(), arrayIndex);
    }

    public bool Remove(TextualToken item)
    {
        return Tokens.Remove(Tokens.First(t => t.Get() == item));
    }

    public IEnumerator<TextualToken> GetEnumerator()
    {
        return Tokens.Select(t => t.Get()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(TextualToken item)
    {
        return Tokens.FindIndex(t => t.Get() == item);
    }

    public void Insert(int index, TextualToken item)
    {
        Tokens.Insert(index, new GuardedLexerToken(item, Expected));
    }

    public void RemoveAt(int index)
    {
        Tokens.RemoveAt(index);
    }
}
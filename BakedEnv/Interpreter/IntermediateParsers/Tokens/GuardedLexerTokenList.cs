using System.Collections;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Tokens;

public class GuardedLexerTokenList : IList<LexerToken>
{
    private LexerTokenType[] Expected { get; }
    private List<GuardedLexerToken> Tokens { get; }

    public int Count => Tokens.Count;
    public bool IsReadOnly => false;

    public GuardedLexerTokenList(LexerTokenType expected)
    {
        Expected = new[] { expected };
        Tokens = new List<GuardedLexerToken>();
    }
    
    public GuardedLexerTokenList(params LexerTokenType[] expected)
    {
        Expected = expected;
        Tokens = new List<GuardedLexerToken>();
    }
    
    public LexerToken this[int index]
    {
        get => Tokens[index].Get();
        set => Tokens[index] = new GuardedLexerToken(value);
    }

    public void Add(LexerToken token)
    {
        Tokens.Add(new GuardedLexerToken(token, Expected));
    }

    public void Clear()
    {
        Tokens.Clear();
    }

    public bool Contains(LexerToken item)
    {
        return Tokens.Any(t => t.Get() == item);
    }

    public void CopyTo(LexerToken[] array, int arrayIndex)
    {
        Tokens.CopyTo(array.Select(t => new GuardedLexerToken(t, Expected)).ToArray(), arrayIndex);
    }

    public bool Remove(LexerToken item)
    {
        return Tokens.Remove(Tokens.First(t => t.Get() == item));
    }

    public IEnumerator<LexerToken> GetEnumerator()
    {
        return Tokens.Select(t => t.Get()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(LexerToken item)
    {
        return Tokens.FindIndex(t => t.Get() == item);
    }

    public void Insert(int index, LexerToken item)
    {
        Tokens.Insert(index, new GuardedLexerToken(item, Expected));
    }

    public void RemoveAt(int index)
    {
        Tokens.RemoveAt(index);
    }
}
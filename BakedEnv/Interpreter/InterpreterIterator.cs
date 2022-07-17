using System.Diagnostics.CodeAnalysis;
using BakedEnv.Common;
using TokenCs;

namespace BakedEnv.Interpreter;

internal class InterpreterIterator : EnumerableIterator<LexerToken>
{
    public BacklogEnumerable<LexerToken> Backlog { get; }

    public InterpreterIterator(IEnumerable<LexerToken> enumerable) : this(new BacklogEnumerable<LexerToken>(enumerable))
    {
        
    }

    public InterpreterIterator(BacklogEnumerable<LexerToken> backlog) : base(backlog)
    {
        Backlog = backlog;
    }

    public bool TrySkip()
    {
        return TryMoveNext(out _);
    }

    public bool TryPeekNext([NotNullWhen(true)] out LexerToken? token)
    {
        if (!TryMoveNext(out token))
            return false;
        
        Backlog.Push(token);
        
        return true;
    }

    public IEnumerable<LexerToken> PeekTakeWhile(Func<LexerToken, bool> predicate)
    {
        while (TryMoveNext(out var token))
        {
            if (!predicate.Invoke(token))
            {
                Backlog.Push(token);
                
                yield break;
            }

            yield return token;
        }
    }

    public void PushCurrent()
    {
        Backlog.Push(Current);
    }
}
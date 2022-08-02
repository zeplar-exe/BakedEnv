using System.Diagnostics.CodeAnalysis;
using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

using TokenCs;

namespace BakedEnv.Interpreter;

internal class InterpreterIterator : EnumerableIterator<IntermediateToken>
{
    public BacklogEnumerable<IntermediateToken> Backlog { get; }

    public InterpreterIterator(IEnumerable<IntermediateToken> enumerable) :
        this(new BacklogEnumerable<IntermediateToken>(enumerable))
    {
        
    }

    public InterpreterIterator(BacklogEnumerable<IntermediateToken> backlog) : base(backlog)
    {
        Backlog = backlog;
    }

    public bool TrySkip()
    {
        return TryMoveNext(out _);
    }

    public bool TryPeekNext([NotNullWhen(true)] out IntermediateToken? token)
    {
        if (!TryMoveNext(out token))
            return false;
        
        Backlog.Push(token);
        
        return true;
    }

    public IEnumerable<IntermediateToken> PeekTakeWhile(Func<IntermediateToken, bool> predicate)
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
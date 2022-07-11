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

    public void PushCurrent()
    {
        Backlog.Push(Current);
    }
}
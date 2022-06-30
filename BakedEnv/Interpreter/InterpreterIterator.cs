using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter;

public class InterpreterIterator : EnumerableIterator<LexerToken>
{
    public BacklogEnumerable<LexerToken> Backlog { get; }

    public InterpreterIterator(IEnumerable<LexerToken> enumerable) : base(new BacklogEnumerable<LexerToken>(enumerable))
    {
        
    }

    public InterpreterIterator(BacklogEnumerable<LexerToken> backlog) : base(backlog)
    {
        Backlog = backlog;
    }
}
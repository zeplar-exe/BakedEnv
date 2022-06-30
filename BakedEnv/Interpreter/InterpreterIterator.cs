using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter;

public class InterpreterIterator : EnumerableIterator<LexerToken>
{
    public BacklogEnumerable<LexerToken> Backlog { get; }

    private InterpreterIterator(BacklogEnumerable<LexerToken> backlog) : base(backlog)
    {
        Backlog = backlog;
    }
}
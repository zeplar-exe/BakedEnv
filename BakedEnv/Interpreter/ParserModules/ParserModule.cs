using BakedEnv.Interpreter.Parsers;

namespace BakedEnv.Interpreter.ParserModules;

internal abstract class ParserModule : IDisposable
{
    protected InterpreterInternals Internals { get; }

    public ParserModule(InterpreterInternals internals)
    {
        Internals = internals;
        Internals.ParserStack.Push(this);
    }

    public void Dispose()
    {
        Internals.ParserStack.Pop();
    }
}
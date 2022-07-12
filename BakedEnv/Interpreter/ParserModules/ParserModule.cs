using BakedEnv.Interpreter.Parsers;

namespace BakedEnv.Interpreter.ParserModules;

internal abstract class ParserModule
{
    protected InterpreterInternals Internals { get; }

    public ParserModule(InterpreterInternals internals)
    {
        Internals = internals;
    }
}
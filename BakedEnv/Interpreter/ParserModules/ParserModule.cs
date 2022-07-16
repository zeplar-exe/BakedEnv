using BakedEnv.Interpreter.Parsers;

namespace BakedEnv.Interpreter.ParserModules;

internal abstract class ParserModule
{
    protected ParserEnvironment Internals { get; }

    public ParserModule(ParserEnvironment internals)
    {
        Internals = internals;
    }
}
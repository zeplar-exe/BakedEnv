using BakedEnv.Interpreter.IntermediateParsers.Errors;

namespace BakedEnv.Interpreter.IntermediateParsers;

public abstract class ParserBase
{
    protected ErrorReporter Error { get; }

    public ParserBase()
    {
        Error = new ErrorReporter();
    }
    
    protected T CreateParser<T>() where T : ParserBase, new()
    {
        var parser = new T();
        
        RegisterParser(parser);

        return parser;
    }

    protected void RegisterParser(ParserBase parser)
    {
        
    }
}
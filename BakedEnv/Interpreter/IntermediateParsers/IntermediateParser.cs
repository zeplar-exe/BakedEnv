using BakedEnv.Common;

namespace BakedEnv.Interpreter.IntermediateParsers;

public abstract class IntermediateParser
{
    private List<BakedError> Errors { get; }
    protected ErrorReporter Error { get; private set; }

    protected IntermediateParser()
    {
        Errors = new List<BakedError>();
        Error = new ErrorReporter();
    }
    
    protected T CreateParser<T>() where T : IntermediateParser, new()
    {
        var parser = new T();

        RegisterParser(parser);

        return parser;
    }

    protected void RegisterParser(IntermediateParser intermediateParser)
    {
        intermediateParser.Error = Error;
    }
}
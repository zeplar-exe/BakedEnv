using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateParsers.Common;

namespace BakedEnv.Interpreter.IntermediateParsers;

public abstract class ParserBase
{
    private List<BakedError> Errors { get; }
    protected ErrorReporter Error { get; private set; }

    protected ParserBase()
    {
        Errors = new List<BakedError>();
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
        parser.Error = Error;
    }
}
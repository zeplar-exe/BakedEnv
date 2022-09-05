namespace BakedEnv.Interpreter.IntermediateParsers;

public abstract class ParserBase
{
    // Nothing yet, plan to implement a parser tree to keep track of context
    
    protected T CreateParser<T>() where T : ParserBase, new()
    {
        var parser = new T();

        return parser;
    }

    protected void RegisterParser(ParserBase parser)
    {
        
    }
}
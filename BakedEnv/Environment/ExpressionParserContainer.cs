using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Environment;

public class ExpressionParserContainer
{
    private List<ExternalExpressionParser> Parsers { get; }

    public ExpressionParserContainer()
    {
        Parsers = new List<ExternalExpressionParser>();
    }

    public void Add(int priority, SingleExpressionParser parser)
    {
        Parsers.Add(new ExternalExpressionParser(priority, parser));
    }
    
    internal IEnumerable<ExternalExpressionParser> EnumerateParsers()
    {
        return Parsers.AsEnumerable();
    }

    internal record ExternalExpressionParser(int Priority, SingleExpressionParser Parser);
}
using BakedEnv.Common;
using BakedEnv.Environment;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Variables;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionSelector
{
    private List<SingleExpressionParser> ExpressionParsers { get; }

    public ExpressionSelector(BakedEnvironment? environment)
    {
        ExpressionParsers = new List<SingleExpressionParser>
        {
            new StringExpressionParser(),
            new IntegerExpressionParser(),
            new DecimalExpressionParser(),
            new IdentifierExpressionParser(),
            new ParenthesisExpressionParser()
        };

        if (environment == null)
            return;
        
        foreach (var parser in environment.ExpressionParsers.EnumerateParsers())
        {
            // Priorities less than 0 denote placement at the end of the list
            if (parser.Priority < 0)
                ExpressionParsers.Add(parser.Parser);
            else
                ExpressionParsers.Insert(parser.Priority, parser.Parser);
        }
    }
    
    public SingleExpressionParser? SelectParser(IntermediateToken token)
    {
        return ExpressionParsers.FirstOrDefault(parser => parser.AllowToken(token));
    }
}
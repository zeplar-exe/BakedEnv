using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Pure;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Variables;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

public class ExpressionSelector
{
    private TypeInstanceList<SingleExpressionParser> ExpressionParsers { get; }

    public ExpressionSelector()
    {
        ExpressionParsers = new TypeInstanceList<SingleExpressionParser>();
        
        ExpressionParsers.Add<StringExpressionParser>();
        ExpressionParsers.Add<IntegerExpressionParser>();
        ExpressionParsers.Add<DecimalExpressionParser>();
    }
    
    public SingleExpressionParser? SelectParser(IntermediateToken token)
    {
        return ExpressionParsers.EnumerateInstances().FirstOrDefault(parser => parser.AllowToken(token));
    }
}
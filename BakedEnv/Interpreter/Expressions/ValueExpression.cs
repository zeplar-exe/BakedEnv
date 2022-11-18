using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class ValueExpression : BakedExpression, IConstantExpression
{
    public BakedObject Value { get; }
    
    public ValueExpression(BakedObject value)
    {
        Value = value;
    }
    
    public override BakedObject Evaluate(InvocationContext context)
    {
        return Value;
    }

    public BakedObject EvaluateConstant()
    {
        return Value;
    }
}
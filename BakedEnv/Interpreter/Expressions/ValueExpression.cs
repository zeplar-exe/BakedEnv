using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class ValueExpression : BakedExpression
{
    public BakedObject Value { get; }
    
    public ValueExpression(BakedObject value)
    {
        Value = value;
    }
    
    public override BakedObject Evaluate(BakedInterpreter interpreter, InvocationContext context)
    {
        return Value;
    }
}
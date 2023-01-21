using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class MemberAccessExpression : BakedExpression
{
    public BakedExpression Expression { get; }
    public string Member { get; }

    public MemberAccessExpression(BakedExpression expression, string member)
    {
        Expression = expression;
        Member = member;
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var value = Expression.Evaluate(context);
        
        return value.TryGetContainedObject(Member, out var member) ? member : new BakedNull();
    }
}
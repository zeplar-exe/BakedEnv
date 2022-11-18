using BakedEnv.Extensions;
using BakedEnv.Helpers;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

public class IndexAssignmentInstruction : InterpreterInstruction
{
    public BakedExpression Target { get; set; }
    public BakedExpression[] IndexValues { get; set; }
    public BakedExpression Value { get; set; }
    
    public IndexAssignmentInstruction(
        BakedExpression target, 
        BakedExpression[] indexValues,
        BakedExpression value,
        ulong sourceIndex) : base(sourceIndex)
    {
        Target = target;
        IndexValues = indexValues;
        Value = value;
    }

    public override void Execute(InvocationContext context)
    {
        var target = Target.Evaluate(context);
        var value = Value.Evaluate(context);
        var indices = IndexValues.Select(i => i.Evaluate(context)).ToArray();

        if (!target.TrySetIndex(indices, value))
        {
            context.ReportError(BakedError.EInvalidIndexAssignment(
                value.TypeName(), StringHelper.CreateTypeList(indices), target.TypeName(), context.SourceIndex));
        }
    }
}
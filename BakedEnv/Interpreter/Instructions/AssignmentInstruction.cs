using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// An instruction to attempt a variable assignment.
/// </summary>
public class AssignmentInstruction : InterpreterInstruction
{
    /// <summary>
    /// Reference object for accessing the variable.
    /// </summary>
    public IAssignableExpression Assignable { get; set; }
    /// <summary>
    /// The expression to assign.
    /// </summary>
    public BakedExpression Expression { get; set; }

    /// <summary>
    /// Initialize a AssignmentInstruction with its 
    /// </summary>
    /// <param name="reference">Reference object for accessing the variable.</param>
    /// <param name="expression">The expression to assign.a</param>
    /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public AssignmentInstruction(IAssignableExpression assignable, BakedExpression expression, long sourceIndex) : base(sourceIndex)
    {
        Assignable = assignable;
        Expression = expression;
    }

    /// <inheritdoc />
    public override void Execute(InvocationContext context)
    {
        if (!Assignable.TryAssign(Expression, context))
        {
            // TODO: ??
        }
    }
}
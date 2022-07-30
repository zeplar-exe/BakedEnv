using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Variables;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// An instruction to attempt a variable assignment.
/// </summary>
public class VariableAssignmentInstruction : InterpreterInstruction
{
    /// <summary>
    /// Reference object for accessing the variable.
    /// </summary>
    public VariableReference Reference { get; set; }
    /// <summary>
    /// The expression to assign.
    /// </summary>
    public BakedExpression Expression { get; set; }

    /// <summary>
    /// Initialize a VariableAssignmentInstruction with its 
    /// </summary>
    /// <param name="reference">Reference object for accessing the variable.</param>
    /// <param name="expression">The expression to assign.a</param>
    /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public VariableAssignmentInstruction(VariableReference reference, BakedExpression expression, int sourceIndex) : base(sourceIndex)
    {
        Reference = reference;
        Expression = expression;
    }

    /// <inheritdoc />
    public override void Execute(InvocationContext context)
    {
        if (!Reference.TrySetVariable(Expression.Evaluate(context)))
        {
            // TODO: ??
        }
    }
}
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;

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
    /// The value to assign.
    /// </summary>
    public BakedObject Value { get; set; }

    /// <summary>
    /// Initialize a VariableAssignmentInstruction with its 
    /// </summary>
    /// <param name="reference">Reference object for accessing the variable.</param>
    /// <param name="value">The value to assign.a</param>
    /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public VariableAssignmentInstruction(VariableReference reference, BakedObject value, int sourceIndex = -1) : base(sourceIndex)
    {
        Reference = reference;
        Value = value;
    }

    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        if (!Reference.TrySetVariable(Value))
        {
            // TODO: ??
        }
    }
}
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// An 'instruction' which is usually placed alongside a <see cref="BakedEnv.BakedError"/>.
/// </summary>
public class InvalidInstruction : InterpreterInstruction
{
    /// <summary>
    /// Error associated with this invalid instruction.
    /// </summary>
    public BakedError AssociatedError { get; }

    /// <summary>
    /// Initialize an InvalidInstruction with its associated error.
    /// </summary>
    /// <param name="error"></param>
    public InvalidInstruction(BakedError error) : base(error.SourceIndex)
    {
        AssociatedError = error;
    }
    
    /// <inheritdoc />
    public override void Execute(InvocationContext context)
    {
        context.Interpreter.Error.Report(AssociatedError);
    }
}
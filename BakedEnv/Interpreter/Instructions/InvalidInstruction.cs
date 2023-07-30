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
    public BakedError[] AssociatedErrors { get; }

    public InvalidInstruction(BakedError error) : base(error.SourceIndex)
    {
        AssociatedErrors = new[] { error };
    }
    
    /// <summary>
    /// Initialize an InvalidInstruction with its associated errors.
    /// </summary>
    /// <param name="errors"></param>
    public InvalidInstruction(BakedError[] errors) : base(errors.Min(e => e.SourceIndex))
    {
        AssociatedErrors = errors;
    }
    
    /// <inheritdoc />
    public override void Execute(InvocationContext context)
    {
        foreach (var error in AssociatedErrors)
        {
            context.Interpreter.Error.Report(error);
        }
    }
}
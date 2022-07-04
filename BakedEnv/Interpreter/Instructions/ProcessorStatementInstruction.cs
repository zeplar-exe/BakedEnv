using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// Instruction used in the handling of processor statements.
/// </summary>
/// <remarks>Usually used internally through <see cref="IProcessorStatementHandler"/>.</remarks>
public class ProcessorStatementInstruction : InterpreterInstruction
{
    /// <summary>
    /// The key/name of this processor statement.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The value of this processor statement.
    /// </summary>
    public BakedObject Value { get; set; }
    
    /// <summary>
    /// Initialize a ProcessorStatementInstruction.
    /// </summary>
    /// <param name="name">The key/name of this processor statement.</param>
    /// <param name="value">The value of this processor statement.</param>
    /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public ProcessorStatementInstruction(string name, BakedObject value, int sourceIndex) : base(sourceIndex)
    {
        Name = name;
        Value = value;
    }

    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        if (interpreter.Environment == null)
            return;
        
        foreach (var handler in interpreter.Environment.ProcessorStatementHandlers)
        {
            if (handler.TryHandle(this, interpreter))
                return;
        }
        
        interpreter.ReportError(CreateInvalidStatementError());
    }

    private BakedError CreateInvalidStatementError()
    {
        return new BakedError(
            ErrorCodes.UnregisteredProcStatement,
            $"A processor statement by the name of '{Name}' does not exist or has not been registered.",
            SourceIndex);
    }
}
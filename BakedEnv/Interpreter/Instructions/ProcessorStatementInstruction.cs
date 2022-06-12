using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class ProcessorStatementInstruction : InterpreterInstruction
{
    public string Name { get; set; }
    public BakedObject Value { get; set; }
    
    public ProcessorStatementInstruction(int sourceIndex) : base(sourceIndex)
    {
        
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
            null,
            $"A processor statement by the name of '{Name}' does not exist or has not been registered.",
            SourceIndex);
    }
}
namespace BakedEnv.Interpreter.Instructions;

public class ProcessorStatementInstruction : InterpreterInstruction
{
    public string Name { get; set; }
    public object Value { get; set; }
    
    public ProcessorStatementInstruction(int sourceIndex) : base(sourceIndex)
    {
        
    }

    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        foreach (var handler in interpreter.ProcessorStatementHandlers)
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
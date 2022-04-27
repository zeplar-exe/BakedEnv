namespace BakedEnv.Interpreter.Instructions;

public class InvalidInstruction : InterpreterInstruction
{
    public InvalidInstruction(int sourceIndex) : base(sourceIndex)
    {
        
    }
    
    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        
    }
}
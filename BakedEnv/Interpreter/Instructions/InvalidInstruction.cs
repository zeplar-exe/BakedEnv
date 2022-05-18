using Jammo.ParserTools;

namespace BakedEnv.Interpreter.Instructions;

public class InvalidInstruction : InterpreterInstruction
{
    public BakedError AssociatedError { get; }
    
    public InvalidInstruction(BakedError error) : base(error.SourceIndex)
    {
        AssociatedError = error;
    }
    
    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        
    }
}
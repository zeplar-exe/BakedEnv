namespace BakedEnv.Interpreter.Instructions;

public class EmptyInstruction : InterpreterInstruction
{
    public EmptyInstruction(int sourceIndex) : base(sourceIndex)
    {
        
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        
    }
}
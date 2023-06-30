using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

public class EmptyInstruction : InterpreterInstruction
{
    public EmptyInstruction(long sourceIndex) : base(sourceIndex)
    {
        
    }

    public override void Execute(InvocationContext context)
    {
        
    }
}
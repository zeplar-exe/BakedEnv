using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

public class EmptyInstruction : InterpreterInstruction
{
    public EmptyInstruction(ulong sourceIndex) : base(sourceIndex)
    {
        
    }

    public override void Execute(InvocationContext context)
    {
        
    }
}
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

public class EndOfFileInstruction : InterpreterInstruction
{
    public EndOfFileInstruction(ulong sourceIndex) : base(sourceIndex)
    {
        
    }

    public override void Execute(InvocationContext context)
    {
        
    }
}
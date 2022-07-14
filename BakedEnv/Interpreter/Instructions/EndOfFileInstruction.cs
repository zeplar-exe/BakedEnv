using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

public class EndOfFileInstruction : InterpreterInstruction
{
    public EndOfFileInstruction(int sourceIndex) : base(sourceIndex)
    {
        
    }

    public override void Execute(InvocationContext context)
    {
        
    }
}
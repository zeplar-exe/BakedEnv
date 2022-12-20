using BakedEnv.Interpreter.Instructions;

namespace BakedEnv;

public partial record struct BakedError
{
    public InvalidInstruction ToInstruction()
    {
        return new InvalidInstruction(this);
    }
}
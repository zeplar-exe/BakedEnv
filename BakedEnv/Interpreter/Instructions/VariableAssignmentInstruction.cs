using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class VariableAssignmentInstruction : InterpreterInstruction
{
    public VariableReference Reference { get; set; }
    public BakedObject Value { get; set; }

    public VariableAssignmentInstruction(int sourceIndex) : base(sourceIndex)
    {
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        if (!Reference.TrySetValue(scope, Value))
        {
            // TODO: ??
        }
    }
}
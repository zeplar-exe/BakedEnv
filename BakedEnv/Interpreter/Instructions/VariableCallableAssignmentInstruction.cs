using BakedEnv.Interpreter.Variables;

namespace BakedEnv.Interpreter.Instructions;

public class VariableCallableAssignmentInstruction : InterpreterInstruction
{
    public VariableReference Reference { get; }
    public ObjectInvocationInstruction InvocationInstruction { get; }
    
    public VariableCallableAssignmentInstruction(VariableReference reference, ObjectInvocationInstruction invocationInstruction, int sourceIndex) : base(sourceIndex)
    {
        Reference = reference;
        InvocationInstruction = invocationInstruction;
    }
    
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        var result = InvocationInstruction.ExecuteReturn(interpreter, scope);
        
        if (!Reference.TrySetVariable(result))
        {
            // TODO: Report error
        }
    }
}
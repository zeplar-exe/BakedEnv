using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class ObjectInvocationInstruction : InterpreterInstruction
{
    public BakedObject Object { get; }
    public BakedObject[] Parameters { get; }

    public ObjectInvocationInstruction(int sourceIndex, BakedObject @object, BakedObject[] parameters) : base(sourceIndex)
    {
        Object = @object;
        Parameters = parameters;
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        Object.TryInvoke(interpreter, scope, out var returnValue);
    }
}
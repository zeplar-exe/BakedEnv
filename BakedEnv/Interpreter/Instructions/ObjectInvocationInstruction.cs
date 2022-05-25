using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class ObjectInvocationInstruction : InterpreterInstruction
{
    public IBakedCallable Callable { get; }
    public BakedObject[] Parameters { get; }

    public ObjectInvocationInstruction(int sourceIndex, IBakedCallable callable, BakedObject[] parameters) : base(sourceIndex)
    {
        Callable = callable;
        Parameters = parameters;
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        Callable.Invoke(Parameters, interpreter, scope);
    }
}
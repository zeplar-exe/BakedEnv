using System.Reflection;

namespace BakedEnv.Interpreter.Instructions;

public class MethodInfoInstruction : InterpreterInstruction
{
    public object Target { get; }
    public MethodInfo MethodInfo { get; }
    public object?[] Parameters { get; }

    public MethodInfoInstruction(MethodInfo methodInfo, params object?[] parameters) : base(-1)
    {
        MethodInfo = methodInfo;
        Parameters = parameters;
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        MethodInfo.Invoke(Target, Parameters);
    }
}
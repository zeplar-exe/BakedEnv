using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;

namespace BakedEnv.Objects;

/// <summary>
/// A defined method.
/// </summary>
public class BakedMethod : BakedObject
{
    /// <summary>
    /// Expected parameters to use during invocation.
    /// </summary>
    public List<ParameterDefinition> ExpectedParameters { get; }
    public List<InterpreterInstruction> Instructions { get; }

    public BakedMethod(IEnumerable<ParameterDefinition> expectedParameters)
    {
        ExpectedParameters = expectedParameters.ToList();
        Instructions = new List<InterpreterInstruction>();
    }

    /// <inheritdoc />
    public override object? GetValue()
    {
        return null;
    }

    /// <inheritdoc />
    public override bool TryInvoke(BakedInterpreter interpreter, IBakedScope scope, out BakedObject returnValue)
    {
        returnValue = Invoke(interpreter, scope);

        return true;
    }
    
    public BakedObject Invoke(BakedInterpreter interpreter, IBakedScope scope)
    {
        foreach (var instruction in Instructions)
        {
            if (instruction is IScriptTermination termination)
                return termination.ReturnValue;
            
            instruction.Execute(interpreter, scope);
        }

        return new BakedVoid();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Empty;
    }
}
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;

namespace BakedEnv.Objects;

/// <summary>
/// A defined method.
/// </summary>
public class BakedMethod : BakedObject, IBakedCallable
{
    /// <summary>
    /// Expected parameters to use during invocation.
    /// </summary>
    public List<string> ParameterNames { get; }
    /// <summary>
    /// List of instructions to be executed during invocation.
    /// </summary>
    public List<InterpreterInstruction> Instructions { get; }

    /// <summary>
    /// Initialize this method with a set of parameter names.
    /// </summary>
    /// <param name="parameterNames">Parameter names inserted into the execution scope.</param>
    public BakedMethod(IEnumerable<string> parameterNames)
    {
        ParameterNames = parameterNames.ToList();
        Instructions = new List<InterpreterInstruction>();
    }

    /// <inheritdoc />
    public override object? GetValue()
    {
        return null;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Instructions.GetHashCode();
    }

    /// <summary>
    /// Invoke this method.
    /// </summary>
    /// <param name="parameters">Parameters aligning with the set <see cref="ParameterNames"/>.</param>
    /// <param name="interpreter">The target interpreter.</param>
    /// <param name="scope">The target scope to execute instructions in.</param>
    /// <returns></returns>
    public BakedObject Invoke(BakedObject[] parameters, BakedInterpreter interpreter, IBakedScope scope)
    {
        for (var paramIndex = 0; paramIndex < parameters.Length && paramIndex < ParameterNames.Count; paramIndex++)
        {
            var param = parameters[paramIndex];
            var paramName = ParameterNames[paramIndex];

            scope.Variables[paramName] = param;
        }
        
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
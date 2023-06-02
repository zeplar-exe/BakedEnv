using BakedEnv.Environment;
using BakedEnv.Environment.ProcessVariables;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Variables;

namespace BakedEnv.Objects;

/// <summary>
/// A defined method.
/// </summary>
public class BakedFunction : BakedObject, IBakedCallable
{
    public static EnvironmentProcessVariable<FunctionScoping> ScopingVariable { get; } = new();

    /// <summary>
    /// Expected parameters to use during invocation.
    /// </summary>
    public List<string> ParameterNames { get; }
    /// <summary>
    /// List of instructions to be executed during invocation.
    /// </summary>
    public List<InterpreterInstruction> Instructions { get; }
    
    public IBakedScope DeclarationScope { get; }

    /// <summary>
    /// Initialize this method with a set of parameter names.
    /// </summary>
    /// <param name="parameterNames">Parameter names inserted into the execution scope.</param>
    public BakedFunction(IEnumerable<string> parameterNames)
    {
        ParameterNames = parameterNames.ToList();
        Instructions = new List<InterpreterInstruction>();
    }

    public static BakedFunction Empty() => new(Enumerable.Empty<string>());

    /// <inheritdoc />
    public override object? GetValue()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Instructions.GetHashCode();
    }

    /// <summary>
    /// Invoke this method.
    /// </summary>
    /// <param name="parameters">NameList aligning with the set <see cref="ParameterNames"/>.</param>
    /// <param name="interpreter">The target interpreter.</param>
    /// <param name="scope">The target scope to execute instructions in.</param>
    /// <returns></returns>
    public BakedObject Invoke(BakedObject[] parameters, InvocationContext context)
    {
        for (var paramIndex = 0; paramIndex < parameters.Length && paramIndex < ParameterNames.Count; paramIndex++)
        {
            var param = parameters[paramIndex];
            var paramName = ParameterNames[paramIndex];

            context.Scope.Variables.Add(new BakedVariable(paramName, param));
        }
        
        var scoping = context.Interpreter.Environment?.GetProcessVariable(ScopingVariable) ?? FunctionScoping.None;
        var instructionScope = scoping switch
        {
            FunctionScoping.None => new BakedScope(null),
            FunctionScoping.Dynamic => context.Scope,
            FunctionScoping.Lexical => DeclarationScope,
            _ => throw new ArgumentOutOfRangeException(nameof(ScopingVariable), scoping.ToString())
        };

        var instructionContext = context with { Scope = instructionScope };
        
        foreach (var instruction in Instructions)
        {
            if (instruction is IScriptTermination termination)
                return termination.ReturnValue;

            instruction.Execute(instructionContext);
        }

        return new BakedVoid();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Empty;
    }
}
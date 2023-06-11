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
    /// <summary>
    /// Expected parameters to use during invocation.
    /// </summary>
    public List<string> ParameterNames { get; }
    /// <summary>
    /// List of instructions to be executed during invocation.
    /// </summary>
    public List<InterpreterInstruction> Instructions { get; }
    
    public FunctionScopeProvider ScopeProvider { get; }

    /// <summary>
    /// Initialize this method with a set of parameter names.
    /// </summary>
    /// <param name="parameterNames">Parameter names inserted into the execution scope.</param>
    public BakedFunction(IEnumerable<string> parameterNames, FunctionScopeProvider scopeProvider)
    {
        ParameterNames = parameterNames.ToList();
        Instructions = new List<InterpreterInstruction>();
        ScopeProvider = scopeProvider;
        
    }

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

        var scope = ScopeProvider.Invoke(context);
        var instructionContext = context with { Scope = scope };
        
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

public class FunctionScopeProvider
{
    private Func<InvocationContext, IBakedScope> Scope { get; }

    public FunctionScopeProvider(Func<InvocationContext, IBakedScope> scope)
    {
        Scope = scope;
    }

    public IBakedScope Invoke(InvocationContext context)
    {
        return Scope.Invoke(context);
    }
    
    public static FunctionScopeProvider None()
    {
        return new FunctionScopeProvider(c => new EmptyScope());
    }
    
    public static FunctionScopeProvider Dynamic()
    {
        return new FunctionScopeProvider(c => c.Scope);
    }
    
    public static FunctionScopeProvider Lexical(IBakedScope declaredScope)
    {
        return new FunctionScopeProvider(c => declaredScope);
    }
}
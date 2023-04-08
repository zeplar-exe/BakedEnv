using System.Collections;

using BakedEnv.Common;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

public class VariableReferenceOrder : IEnumerable<VariableReferenceOrder.VariableReferenceDelegate>
{
    private List<VariableReferenceDelegate> Order { get; }

    public delegate MethodResult<IBakedVariable> VariableReferenceDelegate(
        string name, BakedInterpreter interpreter, IBakedScope scope);
    
    public VariableReferenceOrder()
    {
        Order = new List<VariableReferenceDelegate>();
    }

    public static VariableReferenceOrder Empty() => new();

    public static VariableReferenceOrder Default()
    {
        return new VariableReferenceOrder()
            .Then(VariableReferenceType.Globals)
            .Then(VariableReferenceType.Libraries)
            .Then(VariableReferenceType.ScopeVariables);
    }

    public VariableReferenceOrder Then(VariableReferenceDelegate del)
    {
        Order.Add(del);

        return this;
    }
    
    public VariableReferenceOrder Then(VariableReferenceType type)
    {
        Order.Add(type switch
        {
            VariableReferenceType.Globals => (name, interpreter, scope) =>
            {
                if (interpreter.Environment?.Variables.TryGetValue(name, out var variable) ?? false)
                    return MethodResult<IBakedVariable>.Success(variable);
                
                return MethodResult<IBakedVariable>.Failure();
            },
            VariableReferenceType.Libraries => (name, interpreter, scope) =>
            {
                if (interpreter.Environment == null)
                    return MethodResult<IBakedVariable>.Failure();

                foreach (var library in interpreter.Environment.Libraries)
                {
                    if (library.Variables.TryGetValue(name, out var variable))
                        return MethodResult<IBakedVariable>.Success(variable);
                }
                
                return MethodResult<IBakedVariable>.Failure();
            },
            VariableReferenceType.ScopeVariables => (name, interpreter, scope) =>
            {
                if (scope.Variables.TryGetValue(name, out var variable))
                    return MethodResult<IBakedVariable>.Success(variable);
                
                return MethodResult<IBakedVariable>.Failure();
            },
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        });

    return this;
}

    public IEnumerator<VariableReferenceDelegate> GetEnumerator()
    {
        return Order.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
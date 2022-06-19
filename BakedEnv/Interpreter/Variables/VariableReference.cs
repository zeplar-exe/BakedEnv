using System.Diagnostics.CodeAnalysis;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Variables;

/// <summary>
/// A 'reference' to variables within a BakedInterpreter.
/// </summary>
public class VariableReference
{
    private BakedInterpreter Interpreter { get; }
    
    /// <summary>
    /// The final name of the references variable (a.b.c.Name)
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// The qualifying path of the referenced variable (path1.path2.path3...Name)
    /// </summary>
    /// <remarks>If the path is empty, the variable is assumed to be top-level.</remarks>
    public IReadOnlyCollection<string> Path { get; }

    /// <summary>
    /// Initialize a VariableReference with a full qualifying path.
    /// </summary>
    /// <param name="fullPath">The full path.</param>
    /// <param name="interpreter">The target interpreter.</param>
    /// <exception cref="ArgumentException">The full path is empty.</exception>
    public VariableReference(IEnumerable<string> fullPath, BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
        
        var array = fullPath.ToArray();
        
        if (array.Length < 1)
            throw new ArgumentException("The full path cannot be empty.");

        Name = array.Last();
        Path = array.Take(array.Length - 1).ToList().AsReadOnly();
    }
    
    /// <summary>
    /// Initialize a VariableReference.
    /// </summary>
    /// <param name="name">The referenced variable's name.</param>
    /// <param name="path">The qualifying path/</param>
    /// <param name="interpreter">The target interpreter.</param>
    public VariableReference(string name, IEnumerable<string> path, BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
        
        Name = name;
        Path = path.ToList().AsReadOnly();
    }
    
    /// <summary>
    /// Initialize a top-level variable reference.
    /// </summary>
    /// <param name="name">The referenced variable name.</param>
    /// <param name="interpreter">The target interpreter.</param>
    public VariableReference(string name, BakedInterpreter interpreter)
    {
        Interpreter = interpreter;

        Name = name;
        Path = new List<string>().AsReadOnly();
    }
    
    public bool TryGetVariable([NotNullWhen(true)] out BakedVariable? bakedVariable)
    {
        Interpreter.AssertReady();

        return TryGetVariable(Interpreter.Context, out bakedVariable);
    }
    
    /// <summary>
    /// Get the value (or null) of the referenced variable.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <returns>The value (or null) of the referenced variable.</returns>
    public bool TryGetVariable(IBakedScope scope, [NotNullWhen(true)] out BakedVariable? bakedVariable)
    {
        bakedVariable = null;

        return TryFindVariable(scope, out bakedVariable);
    }

    public bool TrySetValue(BakedObject value)
    {
        Interpreter.AssertReady();

        return TrySetValue(Interpreter.Context, value);
    }

    /// <summary>
    /// Attempt to set the referenced variable.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <param name="value">Value to assign to the referenced variable.</param>
    /// <returns>Whether the variable could be set (false if the variable is read-only or part of its path does not exist).</returns>
    public bool TrySetValue(IBakedScope scope, BakedObject value)
    {
        Interpreter.AssertReady();
        
        if (Path.Count > 0)
        {
            if (TryFindVariable(scope, out var variable))
            {
                variable.Value = value;

                return true;
            }

            return false;
        }
        else
        {
            Interpreter.Context.Variables.Add(Name, value);
            
            return true;
        }
    }
    
    public bool VariableExists()
    {
        Interpreter.AssertReady();
        
        return TryFindVariable(Interpreter.Context, out _);
    }

    public bool VariableExists(IBakedScope scope)
    {
        return TryFindVariable(scope, out _);
    }

    public bool TryFindVariable([NotNullWhen(true)] out BakedVariable? bakedVariable)
    {
        Interpreter.AssertReady();
        
        return TryFindVariable(Interpreter.Context, out bakedVariable);
    }

    public bool TryFindVariable(IBakedScope scope, [NotNullWhen(true)] out BakedVariable? bakedVariable)
    {
        bakedVariable = null;
        
        if (Path.Count > 0)
        {
            return TryFindVariable(scope, out bakedVariable);
        }
        else
        {
            foreach (var variableType in GetReferenceOrder())
            {
                switch (variableType)
                {
                    case VariableReferenceType.Globals:
                        if (Interpreter.Environment?.GlobalVariables.TryGetValue(Name, out bakedVariable) ?? false)
                        {
                            return true;
                        }
                        
                        break;
                    case VariableReferenceType.ScopeVariables:
                        if (scope.Variables.TryGetValue(Name, out bakedVariable))
                        {
                            return true;
                        }
                        
                        break;
                }
            }
        }

        return false;
    }

    public bool TryFindPathObject([NotNullWhen(true)] out BakedObject? bakedObject)
    {
        Interpreter.AssertReady();
        
        return TryFindPathObject(Interpreter.Context, out bakedObject);
    }

    public bool TryFindPathObject(IBakedScope scope, [NotNullWhen(true)] out BakedObject? bakedObject)
    {
        bakedObject = null;
        
        if (Path.Count == 0)
            return false;
        
        var first = Path.First();

        BakedObject? targetObject = null;
        
        foreach (var variableType in GetReferenceOrder())
        {
            switch (variableType)
            {
                case VariableReferenceType.Globals:
                {
                    if (Interpreter.Environment?.GlobalVariables.TryGetValue(first, out var targetVariable) ?? false)
                        targetObject = targetVariable.Value;
                    
                    break;
                }
                case VariableReferenceType.ScopeVariables:
                {
                    var targetScope = scope;

                    BakedVariable? targetVariable;

                    while (!scope.Variables.TryGetValue(first, out targetVariable))
                    {
                        if (targetScope.Parent == null)
                        {
                            return false;
                        }

                        targetScope = targetScope.Parent;
                    }

                    if (targetVariable != null)
                        targetObject = targetVariable.Value;
                    
                    break;
                }
            }
        }

        if (targetObject == null)
            return false;

        foreach (var part in Path.Skip(1))
        {
            if (targetObject.TryGetContainedObject(part, out var contained))
            {
                targetObject = contained;
            }
            else
            {
                return false;
            }
        }

        bakedObject = targetObject;

        return true;
    }

    private VariableReferenceType[] GetReferenceOrder()
    {
        if (Interpreter.Environment != null)
        {
            var order = Interpreter.Environment.VariableReferenceOrder.ToArray();
            
            return order.Length > 0 ? order : new[]
            {
                VariableReferenceType.Globals,
                VariableReferenceType.ScopeVariables
            };
        }

        return new[]
        {
            VariableReferenceType.Globals,
            VariableReferenceType.ScopeVariables
        };
    }
}
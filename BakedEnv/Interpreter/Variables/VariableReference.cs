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

    private IBakedScope? b_scope;

    public IBakedScope Scope
    {
        get
        {
            if (b_scope != null)
                return b_scope;
            
            Interpreter.AssertReady();

            return Interpreter.Context;
        }
        set => b_scope = value;
    }

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

    public VariableReference(IEnumerable<string> fullPath, BakedInterpreter interpreter, IBakedScope scope)
    {
        Interpreter = interpreter;
        
        var array = fullPath.ToArray();
        
        if (array.Length < 1)
            throw new ArgumentException("The full path cannot be empty.");

        Name = array.Last();
        Path = array.Take(array.Length - 1).ToList().AsReadOnly(); 
        Scope = scope;
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

    public VariableReference(string name, IEnumerable<string> path, BakedInterpreter interpreter, IBakedScope scope)
    {
        Interpreter = interpreter;

        Name = name;
        Path = path.ToList().AsReadOnly();
        Scope = scope;
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

    public VariableReference(string name, BakedInterpreter interpreter, IBakedScope scope)
    {
        Interpreter = interpreter;

        Name = name;
        Path = new List<string>().AsReadOnly();
        Scope = scope;
    }

    /// <summary>
    /// Get the referenced (or null).
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <param name="bakedVariable">The value (or null) of the referenced variable.</param>
    /// <returns>Whether the variable was successfully retrieved.</returns>
    public bool TryGetVariable([NotNullWhen(true)] out BakedVariable? bakedVariable)
    {
        bakedVariable = null;

        return TryFindVariable(out bakedVariable);
    }

    /// <summary>
    /// Attempt to set the referenced variable.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <param name="value">Value to assign to the referenced variable.</param>
    /// <returns>Whether the variable could be set (false only if the variable is read-only or part of its path does not exist).</returns>
    public bool TrySetVariable(BakedObject value)
    {
        Interpreter.AssertReady();
        
        if (Path.Count > 0)
        {
            if (TryFindVariable(out var variable))
            {
                if (variable.IsReadOnly)
                    return false;
                    
                variable.Value = value;

                return true;
            }

            return false;
        }
        else
        {
            if (TryFindVariable(out var variable))
            {
                if (variable.IsReadOnly)
                    return false;
                
                variable.Value = value;
            }
            else
                Scope.Variables.Add(Name, value);
            
            return true;
        }
    }

    public bool VariableEquals(object value)
    {
        return TryFindVariable(out var variable) && variable.Value.Equals(value);
    }
    
    public bool VariableEquals(BakedObject value)
    {
        return TryFindVariable(out var variable) && variable.Value.Equals(value);
    }

    /// <summary>
    /// Determine whether the referenced variable exists.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <returns>Whether the referenced variable exists.</returns>
    public bool VariableExists()
    {
        return TryFindVariable(out _);
    }

    /// <summary>
    /// Attempt to find the referenced variable.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <param name="bakedVariable">The referenced variable.</param>
    /// <returns>Whether the variable could be found.</returns>
    public bool TryFindVariable([NotNullWhen(true)] out BakedVariable? bakedVariable)
    {
        bakedVariable = null;
        
        if (Path.Count > 0)
        {
            if (!TryFindPathObject(out var pathObject))
                return false;

            if (!pathObject.TryGetContainedObject(Name, out var bakedObject))
                return false;
            
            bakedVariable = new BakedVariable(Name, bakedObject);

            return true;
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
                        if (Scope.Variables.TryGetValue(Name, out bakedVariable))
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
        bakedObject = null;
        
        if (Path.Count == 0)
            return false;
        
        var first = Path.First();
        var reference = new VariableReference(first, Interpreter);

        if (!reference.TryFindVariable(out var variable))
            return false;

        BakedObject targetObject = variable.Value;

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
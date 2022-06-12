using System.Collections.ObjectModel;
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
    
    /// <summary>
    /// Get the value (or null) of the referenced variable.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <returns>The value (or null) of the referenced variable.</returns>
    public BakedObject GetValue(IBakedScope scope)
    {
        if (Path.Count > 0)
        {
            var first = Path.First();

            BakedObject? targetObject = new BakedNull();

            if (!Interpreter.Environment?.ReadOnlyGlobalVariables.TryGetValue(first, out targetObject) ?? false)
            {
                if (!Interpreter.Environment?.GlobalVariables.TryGetValue(first, out targetObject) ?? false)
                {
                    var targetScope = scope;

                    while (!scope.Variables.TryGetValue(first, out targetObject))
                    {
                        if (targetScope.Parent == null)
                            return new BakedNull();
                        
                        targetScope = targetScope.Parent;
                    }

                    if (targetObject == null)
                        return new BakedNull();
                }
            }

            if (targetObject == null)
                return new BakedNull();

            foreach (var part in Path.Skip(1))
            {
                if (targetObject.TryGetContainedObject(part, out var contained))
                {
                    targetObject = contained;
                }
                else
                {
                    return new BakedNull();
                }
            }

            if (targetObject.TryGetContainedObject(Name, out var bakedObject))
            {
                return bakedObject;
            }

            return new BakedNull();
        }
        else
        {
            if (Interpreter.Environment?.ReadOnlyGlobalVariables.TryGetValue(Name, out var bakedObject) ?? false)
            {
                return bakedObject;
            }
            
            if (Interpreter.Environment?.GlobalVariables.TryGetValue(Name, out bakedObject) ?? false)
            {
                return bakedObject;
            }
            
            if (Interpreter.Context?.Variables.TryGetValue(Name, out bakedObject) ?? false)
            {
                return bakedObject;
            }
        }

        return new BakedNull();
    }

    /// <summary>
    /// Attempt to set the referenced variable.
    /// </summary>
    /// <param name="scope">The target scope.</param>
    /// <param name="value">Value to assign to the referenced variable.</param>
    /// <returns>Whether the variable could be set (false if the variable is read-only or part of its path does not exist).</returns>
    public bool TrySetValue(IBakedScope scope, BakedObject value)
    {
        if (Path.Count > 0)
        {
            var first = Path.First();

            BakedObject? targetObject = null;

            if (Interpreter.Environment.ReadOnlyGlobalVariables.ContainsKey(first))
                return false;
            
            if (!Interpreter.Environment?.GlobalVariables.TryGetValue(first, out targetObject) ?? false)
            {
                var targetScope = scope;

                while (!scope.Variables.TryGetValue(first, out targetObject))
                {
                    if (targetScope.Parent == null)
                        return false;
                        
                    targetScope = targetScope.Parent;
                }

                if (targetObject == null)
                    return false;
            }

            foreach (var part in Path.Skip(1))
            {
                if (targetObject?.TryGetContainedObject(part, out var contained) ?? false)
                {
                    targetObject = contained;
                }
                else
                {
                    return false;
                }
            }

            return targetObject!.TrySetContainedObject(Name, value);
        }
        else
        {
            Interpreter.Context.Variables[Name] = value;
            
            return true;
        }
    }
}
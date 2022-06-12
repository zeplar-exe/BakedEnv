using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Variables;

public class VariableReference
{
    private BakedInterpreter Interpreter { get; }
    
    public string Name { get; }
    public IReadOnlyCollection<string> Path { get; }

    public VariableReference(IEnumerable<string> fullPath, BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
        
        var array = fullPath.ToArray();
        
        if (array.Length < 1)
            return;

        Name = array.Last();
        Path = array.Take(array.Length - 1).ToList().AsReadOnly();
    }
    
    public BakedObject GetValue(IBakedScope scope) // grab from current baked context
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
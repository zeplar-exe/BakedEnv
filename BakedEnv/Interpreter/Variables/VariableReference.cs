using System.Collections.ObjectModel;
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
    
    public bool TryGetValue(out BakedObject? bakedObject)
    {
        bakedObject = null;

        if (Path.Count == 0)
            return false;
        
        if (Path.Count > 1)
        {
            var first = Path.First();
            
            BakedObject? targetObject = null;
            
            if (!Interpreter.Environment?.GlobalVariables.TryGetValue(first, out targetObject) ?? false)
                return false;

            if (!Interpreter.Context?.Variables.TryGetValue(first, out targetObject) ?? false)
                return false;

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

            if (targetObject!.TryGetContainedObject(Name, out bakedObject))
            {
                return true;
            }

            return false;
        }
        else
        {
            if (Interpreter.Environment?.GlobalVariables.TryGetValue(Name, out bakedObject) ?? false)
            {
                return true;
            }
            
            if (Interpreter.Context?.Variables.TryGetValue(Name, out bakedObject) ?? false)
            {
                return true;
            }
        }

        return false;
    }

    public bool TrySetValue(BakedObject value)
    {
        if (Path.Count > 0)
        {
            var first = Path.First();

            BakedObject? targetObject = null;
            
            if (!Interpreter.Environment?.GlobalVariables.TryGetValue(first, out targetObject) ?? false)
                return false;

            if (!Interpreter.Context?.Variables.TryGetValue(first, out targetObject) ?? false)
                return false;

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
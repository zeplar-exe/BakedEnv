using BakedEnv.Objects;

namespace BakedEnv.ApiInjection;

public class ApiObject : BakedObject
{
    public object? Value { get; }
    public Dictionary<string, BakedObject> Properties { get; }

    public ApiObject(object? value)
    {
        Value = value;
        Properties = new Dictionary<string, BakedObject>();
    }
    
    public override object? GetValue()
    {
        return Value;
    }

    public override bool TryGetContainedObject(string name, out BakedObject bakedObject)
    {
        bakedObject = new BakedNull();

        if (!Properties.TryGetValue(name, out var property)) 
            return false;
        
        bakedObject = property;
            
        return true;

    }

    public override bool TrySetContainedObject(string name, BakedObject bakedObject)
    {
        Properties[name] = bakedObject;
        
        return true;
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}
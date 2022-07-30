using BakedEnv.Objects;

namespace BakedEnv.Variables;

public class BakedVariable
{
    public string Name { get; }
    public BakedObject Value { get; set; }
    
    public bool IsReadOnly { get; set; }
    
    public BakedVariable(string name) : this(name, new BakedNull()) { }
    
    public BakedVariable(string name, BakedObject value)
    {
        Name = name;
        Value = value;
    }
}
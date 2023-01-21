using BakedEnv.Objects;

namespace BakedEnv.Variables;

public class BakedVariable : IBakedVariable
{
    private BakedObject b_value;
    
    public string Name { get; }

    public BakedObject Value
    {
        get => b_value;
        set
        {
            if (b_value.Equals(value))
                return;
            
            b_value = value;
            ValueChanged?.Invoke(this, value);
        }
    }

    public VariableFlags Flags { get; set; }
    
    public event VariableChangedHandler? ValueChanged;

    public BakedVariable(string name, VariableFlags flags = 0) : this(name, new BakedNull()) { }
    
    public BakedVariable(string name, BakedObject value, VariableFlags flags = 0)
    {
        Name = name;
        b_value = value;
        Flags = flags;
    }
}
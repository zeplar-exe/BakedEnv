using BakedEnv.Objects;

namespace BakedEnv.Variables;

public interface IBakedVariable
{
    public string Name { get; }
    public BakedObject Value { get; set; }
    public VariableFlags Flags { get; }
    
    public event VariableChangedHandler? ValueChanged;
}
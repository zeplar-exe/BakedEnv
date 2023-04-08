using System.Diagnostics.CodeAnalysis;

using BakedEnv.Objects;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

public class EnvironmentVariableBuilder
{
    private BakedEnvironmentBuilder Source { get; }
    
    private string? Name { get; set; }
    private VariableFlags Flags { get; set; }
    private BakedObject Value { get; set; }

    internal EnvironmentVariableBuilder(BakedEnvironmentBuilder source)
    {
        Source = source;
        Value = new BakedNull();
    }

    public EnvironmentVariableBuilder WithName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;

        return this;
    }

    public EnvironmentVariableBuilder WithValue(BakedObject value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        Value = value;

        return this;
    }

    public EnvironmentVariableBuilder AsReadOnly(bool flag = true)
    {
        if (flag)
            Flags |= VariableFlags.ReadOnly;
        else
            Flags &= ~VariableFlags.ReadOnly;

        return this;
    }

    public BakedEnvironmentBuilder Complete()
    {
        AssertRequiredValue(Name);
        AssertRequiredValue(Value);

        var variable = new BakedVariable(Name, Value, Flags);

        return Source.WithVariable(variable);
    }

    private void AssertRequiredValue([NotNull] object? value)
    {
        if (value == null)
            throw new InvalidOperationException($"Cannot build when a required value is null ({nameof(value)}).");
    }
}
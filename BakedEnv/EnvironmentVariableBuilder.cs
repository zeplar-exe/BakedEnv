using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;

namespace BakedEnv;

public class EnvironmentVariableBuilder
{
    private BakedEnvironmentBuilder Source { get; }
    
    private string? Name { get; set; }
    private VariableAttributes Attributes { get; set; }
    private BakedObject? Value { get; set; }

    internal EnvironmentVariableBuilder(BakedEnvironmentBuilder source)
    {
        Source = source;
    }

    public EnvironmentVariableBuilder WithName(string name)
    {
        Name = name;

        return this;
    }

    public EnvironmentVariableBuilder WithValue(BakedObject value)
    {
        Value = value;

        return this;
    }

    public EnvironmentVariableBuilder AsReadOnly(bool flag = true)
    {
        if (flag)
            Attributes |= VariableAttributes.ReadOnly;
        else
            Attributes &= ~VariableAttributes.ReadOnly;

        return this;
    }

    public BakedEnvironmentBuilder Complete()
    {
        AssertRequiredValue(Name);
        AssertRequiredValue(Value);
        
        var variable = new BakedVariable(Name, Value)
        {
            IsReadOnly = Attributes.HasFlag(VariableAttributes.ReadOnly)
        };

        return Source.WithVariable(variable);
    }

    private void AssertRequiredValue([NotNull] object? value)
    {
        if (value == null)
            throw new InvalidOperationException($"Cannot build when a required value is null ({nameof(value)}).");
    }
}
using BakedEnv.ControlStatements;
using BakedEnv.Environment;
using BakedEnv.Objects;
using BakedEnv.Variables;

namespace BakedEnv.Extensions;

public static class BakedEnvironmentExtensions
{
    /// <summary>
    /// Assign true and false to their respective variables.
    /// </summary>
    public static BakedEnvironmentBuilder WithBooleanVariables(this BakedEnvironmentBuilder environment)
    {
        return environment
            .WithVariable("true", new BakedBoolean(true))
            .WithVariable("false", new BakedBoolean(false));
    }
    
    public static BakedEnvironmentBuilder WithReadOnlyBooleanVariables(this BakedEnvironmentBuilder environment)
    {
        return environment
            .WithVariable(new BakedVariable("true", new BakedBoolean(true)) { IsReadOnly = true })
            .WithVariable(new BakedVariable("false", new BakedBoolean(true)) { IsReadOnly = true });
    }

    public static BakedEnvironmentBuilder WithNullVariable(this BakedEnvironmentBuilder environment)
    {
        return environment
            .WithVariable("null", new BakedNull());
    }
    
    public static BakedEnvironmentBuilder WithReadOnlyNullVariable(this BakedEnvironmentBuilder environment)
    {
        return environment
            .WithVariable(new BakedVariable("null", new BakedNull()) { IsReadOnly = true });
    }

    public static BakedEnvironmentBuilder WithControlFlow(this BakedEnvironmentBuilder environment)
    {
        return environment
            .WithControlStatement(new IfStatementDefinition())
            .WithControlStatement(new WhileStatementDefinition());
    }

    public static BakedEnvironmentBuilder ToBuilder(this BakedEnvironment environment)
    {
        return new BakedEnvironmentBuilder(environment);
    }
}
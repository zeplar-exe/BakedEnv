using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;

namespace BakedEnv.Extensions;

public static class BakedEnvironmentExtensions
{
    /// <summary>
    /// Assign true and false to their respective variables.
    /// </summary>
    public static BakedEnvironment WithBooleanVariables(this BakedEnvironment environment)
    {
        return environment
            .WithVariable("true", new BakedBoolean(true))
            .WithVariable("false", new BakedBoolean(false));
    }
    
    public static BakedEnvironment WithReadOnlyBooleanVariables(this BakedEnvironment environment)
    {
        return environment
            .WithVariable(new BakedVariable("true", new BakedBoolean(true)) { IsReadOnly = true })
            .WithVariable(new BakedVariable("false", new BakedBoolean(true)) { IsReadOnly = true });
    }

    public static BakedEnvironment WithNullVariable(this BakedEnvironment environment)
    {
        return environment
            .WithVariable("null", new BakedNull());
    }
    
    public static BakedEnvironment WithReadOnlyNullVariable(this BakedEnvironment environment)
    {
        return environment
            .WithVariable(new BakedVariable("null", new BakedNull()) { IsReadOnly = true });
    }

    public static BakedEnvironment WithControlFlow(this BakedEnvironment environment)
    {
        return environment
            .WithControlStatement(new IfStatementDefinition())
            .WithControlStatement(new WhileStatementDefinition());
    }
}
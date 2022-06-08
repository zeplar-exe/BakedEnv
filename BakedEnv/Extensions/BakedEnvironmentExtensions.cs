using BakedEnv.Objects;

namespace BakedEnv.Extensions;

public static class BakedEnvironmentExtensions
{
    /// <summary>
    /// Assign true and false to their respective variables.
    /// </summary>
    public static BakedEnvironment WithBooleanVariables(this BakedEnvironment environment)
    {
        environment.GlobalVariables["true"] = new BakedBoolean(true);
        environment.GlobalVariables["false"] = new BakedBoolean(false);

        return environment;
    }
    
    public static BakedEnvironment WithReadOnlyBooleanVariables(this BakedEnvironment environment)
    {
        environment.ReadOnlyGlobalVariables["true"] = new BakedBoolean(true);
        environment.ReadOnlyGlobalVariables["false"] = new BakedBoolean(false);

        return environment;
    }

    public static BakedEnvironment WithNullVariable(this BakedEnvironment environment)
    {
        environment.GlobalVariables["null"] = new BakedNull();

        return environment;
    }
    
    public static BakedEnvironment WithReadOnlyNullVariable(this BakedEnvironment environment)
    {
        environment.ReadOnlyGlobalVariables["null"] = new BakedNull();

        return environment;
    }
}
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
}
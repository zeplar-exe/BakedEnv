namespace BakedEnv;

/// <summary>
/// Type of a BakedEnv script. Determines interpreter behavior.
/// </summary>
public enum BakeType
{
    /// <summary>
    /// The script is executed once.
    /// </summary>
    Script = 0,
    /// <summary>
    /// The script is executed once, and its result is returned upon reaching a return statement or EOF.
    /// </summary>
    Module
}
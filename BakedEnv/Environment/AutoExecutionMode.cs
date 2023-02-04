namespace BakedEnv.Environment;

/// <summary>
/// Determine how <see cref="BakedEnvironment"/> methods will invoke instructions.
/// </summary>
public enum AutoExecutionMode
{
    /// <summary>
    /// Instructions are not executed during iteration.
    /// </summary>
    None = 0,
    /// <summary>
    /// Instructions are executed before the object is yielded to the iterator.
    /// </summary>
    BeforeYield,
}
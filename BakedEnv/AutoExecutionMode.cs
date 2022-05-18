namespace BakedEnv;

/// <summary>
/// Determine how <see cref="BakedEnvironment"/> methods will invoke instructions.
/// </summary>
public enum AutoExecutionMode
{
    /// <summary>
    /// Instructions are not executed during invocation.
    /// </summary>
    None = 0,
    /// <summary>
    /// Instructions are executed before the object is yielded to the iterator.
    /// </summary>
    BeforeYield,
    /// <summary>
    /// Instructions are executed after the object is yielded to the iterator.
    /// </summary>
    AfterYield
}
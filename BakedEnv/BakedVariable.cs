namespace BakedEnv;

/// <summary>
/// Variable declared in a script or externally.
/// </summary>
public class BakedVariable
{
    /// <summary>
    /// Name of the variable.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Value of the variable.
    /// </summary>
    public object? Value { get; set; }
}
namespace BakedEnv;

/// <summary>
/// Variable declared in a script or externally.
/// </summary>
public class BakedVariable
{
    /// <summary>
    /// Value of the variable.
    /// </summary>
    public object? Value { get; set; }

    public BakedVariable()
    {
        
    }

    public BakedVariable(object value)
    {
        Value = value;
    }
}
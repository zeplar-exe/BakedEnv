using System.Reflection;

namespace BakedEnv.ExternalApi;

/// <summary>
/// Outline of an API proeprty.
/// </summary>
public class ApiPropertyNode : IAccessible
{
    /// <summary>
    /// Name of this API property.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Value of this API property.
    /// </summary>
    public ApiPropertyValue Value { get; set; }

    /// <summary>
    /// Instantiate an ApiPropertyNode.
    /// The Value property is automatically supplied.
    /// </summary>
    public ApiPropertyNode()
    {
        Value = new ApiPropertyValue();
    }
    
    object? IAccessible.GetPropertyValue(string name)
    {
        return Value.GetProperty(name)?.Value.Value;
    }
    
    MethodInfo? IAccessible.GetMethod(string name)
    {
        return Value.GetMethod(name)?.Method;
    }
}
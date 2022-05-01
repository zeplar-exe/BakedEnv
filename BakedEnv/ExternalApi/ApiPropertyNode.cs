using System.Reflection;

namespace BakedEnv.ExternalApi;

/// <summary>
/// Outline of an API property.
/// </summary>
public class ApiPropertyNode
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
}
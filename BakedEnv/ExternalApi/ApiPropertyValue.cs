namespace BakedEnv.ExternalApi;

/// <summary>
/// Value of an <see cref="ApiPropertyNode"/>.
/// </summary>
public class ApiPropertyValue
{
    /// <summary>
    /// Raw value.
    /// </summary>
    public object? Value { get; set; }
    
    /// <summary>
    /// Properties under the raw value of this object.
    /// </summary>
    public List<ApiPropertyNode> PropertyNodes { get; }
    /// <summary>
    /// Methods under the raw value of this object.
    /// </summary>
    public List<ApiMethodNode> MethodNodes { get; }
    
    /// <summary>
    /// Shorthand for <code>PropertyNodes.FirstOrDefault(p => p.Name == name)</code>
    /// </summary>
    /// <param name="name">Name of the property to get.</param>
    /// <returns>An <see cref="ApiPropertyNode"/> with the given name, or null.</returns>
    public ApiPropertyNode? GetProperty(string name) => PropertyNodes.FirstOrDefault(p => p.Name == name);
    /// <summary>
    /// Shorthand for <code>MethodNodes.FirstOrDefault(p => p.Name == name)</code>
    /// </summary>
    /// <param name="name">Name of the method to get.</param>
    /// <returns>An <see cref="ApiMethodNode"/> with the given name, or null.</returns>
    public ApiMethodNode? GetMethod(string name) => MethodNodes.FirstOrDefault(p => p.Name == name);

    /// <summary>
    /// Initialize an ApiPropertyValue.
    /// </summary>
    public ApiPropertyValue()
    {
        PropertyNodes = new List<ApiPropertyNode>();
        MethodNodes = new List<ApiMethodNode>();
    }
}
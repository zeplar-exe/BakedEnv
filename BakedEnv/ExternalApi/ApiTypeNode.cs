using System.Reflection;

namespace BakedEnv.ExternalApi;

/// <summary>
/// Root node for <see cref="ApiStructure">ApiStructures</see>.
/// </summary>
public class ApiTypeNode
{
    /// <summary>
    /// Name of this ApiTypeNode.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Top-level method nodes.
    /// </summary>
    public List<ApiMethodNode> MethodNodes { get; }
    /// <summary>
    /// Top-level property nodes.
    /// </summary>
    public List<ApiPropertyNode> PropertyNodes { get; }
    
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
    /// Instantiate an ApiTypeNode.
    /// </summary>
    public ApiTypeNode()
    {
        MethodNodes = new List<ApiMethodNode>();
        PropertyNodes = new List<ApiPropertyNode>();
    }
}
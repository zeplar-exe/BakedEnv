using System.Reflection;

namespace BakedEnv.ExternalApi;

/// <summary>
/// Outline of an API method.
/// </summary>
public class ApiMethodNode
{
    /// <summary>
    /// Name of this API method.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// C#-defined method to be used during invocation.
    /// </summary>
    public MethodInfo? Method { get; set; }
}
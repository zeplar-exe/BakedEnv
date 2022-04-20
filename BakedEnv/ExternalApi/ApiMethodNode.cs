using System.Reflection;

namespace BakedEnv.ExternalApi;

public class ApiMethodNode
{
    public string? Name { get; set; }
    public MethodInfo? Method { get; set; }
}
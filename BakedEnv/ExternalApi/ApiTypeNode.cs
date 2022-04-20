namespace BakedEnv.ExternalApi;

public class ApiTypeNode
{
    public string? Name { get; set; }
    public List<ApiMethodNode> MethodNodes { get; }
    public List<ApiPropertyNode> PropertyNodes { get; }

    public ApiPropertyNode? GetProperty(string name) => PropertyNodes.FirstOrDefault(p => p.Name == name);
    public ApiMethodNode? GetMethod(string name) => MethodNodes.FirstOrDefault(p => p.Name == name);

    public ApiTypeNode()
    {
        MethodNodes = new List<ApiMethodNode>();
        PropertyNodes = new List<ApiPropertyNode>();
    }
}
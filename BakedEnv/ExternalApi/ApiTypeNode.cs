namespace BakedEnv.ExternalApi;

public class ApiTypeNode
{
    public string? Name { get; set; }
    public List<ApiMethodNode> MethodNodes { get; }
    public List<ApiPropertyNode> PropertyNodes { get; }

    public ApiTypeNode()
    {
        MethodNodes = new List<ApiMethodNode>();
        PropertyNodes = new List<ApiPropertyNode>();
    }
}
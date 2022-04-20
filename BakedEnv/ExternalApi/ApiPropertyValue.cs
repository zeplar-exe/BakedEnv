namespace BakedEnv.ExternalApi;

public class ApiPropertyValue
{
    public object? Value { get; set; }
    
    public List<ApiPropertyNode> PropertyNodes { get; }
    public List<ApiMethodNode> MethodNodes { get; }
    
    public ApiPropertyNode? GetProperty(string name) => PropertyNodes.FirstOrDefault(p => p.Name == name);
    public ApiMethodNode? GetMethod(string name) => MethodNodes.FirstOrDefault(p => p.Name == name);

    public ApiPropertyValue()
    {
        PropertyNodes = new List<ApiPropertyNode>();
        MethodNodes = new List<ApiMethodNode>();
    }
}
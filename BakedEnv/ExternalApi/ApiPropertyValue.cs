namespace BakedEnv.ExternalApi;

public class ApiPropertyValue
{
    public object? Value { get; set; }
    
    public List<ApiPropertyNode> PropertyNodes { get; }
    public List<ApiMethodNode> MethodNodes { get; }

    public ApiPropertyValue()
    {
        PropertyNodes = new List<ApiPropertyNode>();
        MethodNodes = new List<ApiMethodNode>();
    }
}
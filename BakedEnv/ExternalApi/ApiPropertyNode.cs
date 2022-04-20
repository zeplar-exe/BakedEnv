namespace BakedEnv.ExternalApi;

public class ApiPropertyNode
{
    public string? Name { get; set; }
    public ApiPropertyValue Value { get; set; }

    public ApiPropertyNode()
    {
        Value = new ApiPropertyValue();
    }
}
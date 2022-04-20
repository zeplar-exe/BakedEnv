namespace BakedEnv.ExternalApi;

public class ApiMethodNode
{
    public string? Name { get; set; }
    
    private object? b_action;
    public object? Action
    {
        get => b_action;
        set
        {
            if (value != null && value.GetType() != typeof(Action<>))
                throw new ArgumentException($"Expected an action, got '{value.GetType().Name}'");

            b_action = value;
        }
    }
}
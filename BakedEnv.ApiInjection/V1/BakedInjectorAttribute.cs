namespace BakedEnv.ApiInjection.V1;

[AttributeUsage(AttributeTargets.Class)]
public class BakedInjectorAttribute : Attribute
{
    public string InjectorMethodName { get; }

    public BakedInjectorAttribute(string injectorMethodName)
    {
        InjectorMethodName = injectorMethodName;
    }
}
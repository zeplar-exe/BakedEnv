namespace BakedEnv.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class BakedPropertyAttribute : Attribute
{
    public string? Name { get; }

    public BakedPropertyAttribute(string? name = null)
    {
        Name = name;
    }
}
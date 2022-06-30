using System.Reflection;
using BakedEnv.Objects;

namespace BakedEnv.ApiInjection;

public class ApiBuilder
{
    private List<ApiBuilder> Children { get; }
    
    public string Name { get; private set; }
    public object? RawValue { get; private set; }
    
    public DelegateObject? Delegate { get; private set; }

    public static ApiBuilder FromObject(object o, BindingFlags flags)
    {
        return FromType(o.GetType(), flags).SyncWith(o);
    }

    public static ApiBuilder FromType(Type type, BindingFlags flags)
    {
        foreach (var property in type.GetProperties(flags))
        {
            
        }

        foreach (var nestedType in type.GetNestedTypes(flags))
        {
            if (nestedType.IsInterface)
                continue;
            
            if (nestedType.IsEnum)
            {
                
            }
            else if (nestedType.IsClass || nestedType.IsValueType)
            {
                
            }
        }

        foreach (var method in type.GetMethods(flags))
        {
            
        }

        return new ApiBuilder();
    }

    public ApiBuilder()
    {
        Children = new List<ApiBuilder>();
    }

    public ApiBuilder WithName(string name)
    {
        Name = name;

        return this;
    }

    public ApiBuilder WithValue(string name, object value)
    {
        return Property(name).WithValue(value);
    }

    public ApiBuilder Property(string name)
    {
        Name = name;
        
        return this;
    }
    
    public ApiBuilder WithValue(object? value)
    {
        RawValue = value;

        return this;
    }

    public ApiBuilder WithDelegate(DelegateObject d)
    {
        Delegate = d;

        return this;
    }

    public ApiBuilder Property()
    {
        var builder = new ApiBuilder();

        return Property(builder);
    }

    public ApiBuilder Property(ApiBuilder property)
    {
        Children.Add(property);

        return property;
    }

    public ApiBuilder Method(string name, DelegateObject d)
    {
        return Method(name).WithDelegate(d);
    }
    
    public ApiBuilder Method(string name)
    {
        return Method().WithName(name);
    }

    public ApiBuilder Method(DelegateObject d)
    {
        return Method().WithDelegate(d);
    }

    public ApiBuilder Method()
    {
        return Method(new ApiBuilder());
    }

    public ApiBuilder Method(ApiBuilder method)
    {
        Children.Add(method);

        return method;
    }

    public ApiBuilder SyncWith(object o)
    {
        return this;
    }

    public BakedObject Build()
    {
        BakedObject root;

        if (Delegate != null)
            root = Delegate;
        else
        {
            var apiObject = new ApiObject(RawValue);
            
            foreach (var child in Children)
            {
                apiObject.Properties[child.Name] = child.Build();
            }

            root = apiObject;
        }

        return root;
    }
}
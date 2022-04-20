using System.Reflection;
using System.Xml.Linq;

namespace BakedEnv.ExternalApi;

public class ApiStructure
{
    public ApiTypeNode Root { get; }
 
    public ApiStructure(ApiTypeNode root)
    {
        Root = root;
    }
    
    public void SyncObject(object o, StructureDiffHandler diffHandler = StructureDiffHandler.None)
    {
        throw new NotImplementedException();
    }
    
    public static ApiStructure FromXml(string file)
    {
        if (!File.Exists(file))
            throw new FileNotFoundException(null, file);

        return FromXml(XDocument.Load(File.OpenRead(file)).Root ?? new XElement(string.Empty));
    }

    public static ApiStructure FromXml(XElement xmlRoot)
    {
        var root = new ApiTypeNode
        {
            Name = xmlRoot.Attribute("name")?.Value
        };

        foreach (var child in xmlRoot.Elements())
        {
            switch (child.Name.ToString())
            {
                case "method":
                    root.MethodNodes.Add(CreateMethodNode(child));
                    break;
                case "property":
                    root.PropertyNodes.Add(CreatePropertyNode(child));
                    break;
            }
        }

        return new ApiStructure(root);
    }

    private static ApiMethodNode CreateMethodNode(XElement element)
    {
        var node = new ApiMethodNode
        {
            Name = element.Attribute("name")?.Value
        };

        return node;
    }

    private static ApiPropertyNode CreatePropertyNode(XElement element)
    {
        var node = new ApiPropertyNode
        {
            Name = element.Attribute("name")?.Value,
            Value =
            {
                Value = element.Attribute("default_value")?.Value
            }
        };

        foreach (var child in element.Elements())
        {
            switch (child.Name.ToString())
            {
                case "method":
                    node.Value.MethodNodes.Add(CreateMethodNode(child));
                    break;
                case "property":
                    node.Value.PropertyNodes.Add(CreatePropertyNode(child));
                    break;
            }
        }

        return node;
    }
    
    public static ApiStructure FromType(Type type, 
        BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
    {
        var root = new ApiTypeNode
        {
            Name = type.Name
        };

        foreach (var method in type.GetMethods(flags))
        {
            root.MethodNodes.Add(CreateMethodNode(method, null));
        }

        foreach (var property in type.GetProperties(flags))
        {
            root.PropertyNodes.Add(CreatePropertyNode(property, null, flags));
        }

        return new ApiStructure(root);
    }

    public static ApiStructure FromObject(object o, 
        BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
    {
        var type = o.GetType();
        var root = new ApiTypeNode
        {
            Name = type.Name
        };

        foreach (var method in type.GetMethods(flags).Where(m => !m.IsSpecialName))
        { // Where clause ignores property get/set methods
            root.MethodNodes.Add(CreateMethodNode(method, o));
        }

        foreach (var property in type.GetProperties(flags))
        {
            root.PropertyNodes.Add(CreatePropertyNode(property, o, flags));
        }

        return new ApiStructure(root);
    }

    private static ApiMethodNode CreateMethodNode(MethodInfo methodInfo, object? target)
    {
        return new ApiMethodNode
        {
            Name = methodInfo.Name,
            Method = methodInfo
        };
    }

    private static ApiPropertyNode CreatePropertyNode(PropertyInfo propertyInfo, object? target, BindingFlags flags)
    {
        var node = new ApiPropertyNode
        {
            Name = propertyInfo.Name,
        };

        if (propertyInfo.GetIndexParameters().Length > 0)
            return node;
        
        var propertyValue = propertyInfo.GetValue(target);

        node.Value.Value = propertyValue;

        if (propertyValue != null)
        {
            foreach (var method in propertyInfo.PropertyType.GetMethods(flags).Where(m => !m.IsSpecialName))
            {
                node.Value.MethodNodes.Add(CreateMethodNode(method, propertyValue));
            }

            foreach (var property in propertyInfo.PropertyType.GetProperties(flags))
            {
                node.Value.PropertyNodes.Add(CreatePropertyNode(property, propertyValue, flags));
            }
        }

        return node;
    }
}
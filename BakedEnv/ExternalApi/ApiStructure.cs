using System.Reflection;
using System.Xml.Linq;

namespace BakedEnv.ExternalApi;

/// <summary>
/// Structure of an API to provide properties and methods. Used during BakedEnv script interpretation.
/// </summary>
public class ApiStructure
{
    /// <summary>
    /// Root node of this structure.
    /// </summary>
    public ApiTypeNode Root { get; }
 
    /// <summary>
    /// Initialize an ApiStructure.
    /// </summary>
    /// <param name="root">The root node of this structure.
    /// It is recommended that you use FromXml, FromObject, or FromType instead of creating one on your own. </param>
    public ApiStructure(ApiTypeNode root)
    {
        Root = root;
    }
    
    /// <summary>
    /// Sync an object's property values and method references to this structure.
    /// </summary>
    /// <param name="o">Object to sync from.</param>
    /// <param name="diffHandler">Flags on how to handle edge cases.</param>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public void SyncObject(object o, StructureDiffHandler diffHandler = StructureDiffHandler.None)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Create an ApiStructure recursively from a .xml file.
    /// </summary>
    /// <param name="file">The target file path.</param>
    /// <returns>A filled out ApiStructure.</returns>
    /// <exception cref="FileNotFoundException">The target file does not exist.</exception>
    public static ApiStructure FromXml(string file)
    {
        if (!File.Exists(file))
            throw new FileNotFoundException(null, file);

        return FromXml(XDocument.Load(File.OpenRead(file)).Root ?? new XElement(string.Empty));
    }

    /// <summary>
    /// Create an ApiStructure recursively from a raw XElement.
    /// </summary>
    /// <param name="xmlRoot">Raw XElement to read from.</param>
    /// <returns>A filled out ApiStructure.</returns>
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

    /// <summary>
    /// Create an ApiStructure from an object.
    /// </summary>
    /// <param name="o">The target object.</param>
    /// <param name="flags"><see cref="BindingFlags"/> used during reflection.</param>
    /// <returns>A filled out ApiStructure.</returns>
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
            var propertyType = propertyValue.GetType();
            
            foreach (var method in propertyType.GetMethods(flags).Where(m => !m.IsSpecialName))
            {
                node.Value.MethodNodes.Add(CreateMethodNode(method, propertyValue));
            }

            foreach (var property in propertyType.GetProperties(flags))
            {
                node.Value.PropertyNodes.Add(CreatePropertyNode(property, propertyValue, flags));
            }
        }

        return node;
    }
}
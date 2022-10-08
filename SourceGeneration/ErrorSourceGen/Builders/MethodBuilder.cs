using System.Text;
using System.Xml.Linq;

namespace ErrorSourceGen.Builders;

public class MethodBuilder
{
    private string Name { get; set; }
    private Accessibility Accessibility { get; set; }
    private Inheritability Inheritability { get; set; }
    private bool IsExtern { get; set; }
    private bool IsOverride { get; set; }
    private bool IsSealed { get; set; }
    private bool IsStatic { get; set; }
    private Dictionary<string, string> Parameters { get; }
    private string ReturnType { get; set; }
    
    public XElement XmlDoc { get; }
    public StringBuilder Body { get; }

    public MethodBuilder()
    {
        Name = string.Empty;
        Parameters = new Dictionary<string, string>();
        Body = new StringBuilder();
    }

    public MethodBuilder WithName(string name)
    {
        Name = name;

        return this;
    }

    public MethodBuilder WithAccessibility(Accessibility accessibility)
    {
        Accessibility = accessibility;
        
        return this;
    }

    public MethodBuilder WithInheritability(Inheritability inheritability)
    {
        Inheritability = inheritability;
        
        return this;
    }

    public MethodBuilder AsExtern(bool isExtern = true)
    {
        IsExtern = isExtern;
        
        return this;
    }

    public MethodBuilder AsOverride(bool isOverride = true)
    {
        IsOverride = isOverride;
        
        return this;
    }

    public MethodBuilder AsSealed(bool isSealed = true)
    {
        IsSealed = isSealed;
        
        return this;
    }

    public MethodBuilder AsStatic(bool isStatic = true)
    {
        IsStatic = isStatic;

        return this;
    }

    public MethodBuilder WithParameter(string name, string type)
    {
        Parameters[name] = type;

        return this;
    }
    
    public MethodBuilder WithParameter((string Name, string Type) parameter)
    {
        return WithParameter(parameter.Name, parameter.Type);
    }
    
    public MethodBuilder WithParameters(IEnumerable<KeyValuePair<string, string>> parameters)
    {
        foreach (var param in parameters)
        {
            WithParameter(param.Key, param.Value);
        }

        return this;
    }
    
    public MethodBuilder WithParameters(IEnumerable<(string Name, string Type)> parameters)
    {
        foreach (var param in parameters)
        {
            WithParameter(param.Name, param.Type);
        }

        return this;
    }

    public MethodBuilder WithReturnType(string returnType)
    {
        ReturnType = returnType;

        return this;
    }
    
    public override string ToString()
    {
        var access = Accessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Internal => "internal",
            Accessibility.Protected => "protected",
            Accessibility.Private => "private",
            Accessibility.PrivateProtected => "private protected",
            Accessibility.ProtectedInternal => "protected internal",
            _ => "public"
        };

        var inherit = Inheritability switch
        {
            Inheritability.None => string.Empty,
            Inheritability.Virtual => "virtual",
            Inheritability.Abstract => "abstract",
            _ => string.Empty
        };

        var parameters = Parameters.Select(p => $"{p.Value} {p.Key}");

        return new StringBuilder()
            .Append(access).Append(' ')
            .Append(inherit).Append(Inheritability == Inheritability.None ? "" : " ")
            .Append(ReturnType).Append(' ')
            .Append(Name).Append('(')
            .Append(string.Join(", ", parameters)).Append(')')
            .Append('{').AppendLine()
            .Append(Body).AppendLine()
            .Append('}').AppendLine()
            .ToString();
    }
}
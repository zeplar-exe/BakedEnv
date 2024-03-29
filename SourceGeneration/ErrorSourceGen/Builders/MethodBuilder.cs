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
        XmlDoc = new XElement("summary");
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

        var doc = new StringBuilder();
        var xml = XmlDoc.ToString();

        var reader = new StringReader(xml);

        while (reader.Peek() != -1)
        {
            doc.Append("/// ");
            doc.Append(reader.ReadLine());
        }

        return new StringBuilder()
            .AppendLine(doc.ToString())
            .Append(access).Append(' ')
            .Append(inherit).Append(Inheritability == Inheritability.None ? "" : " ")
            .Append(IsExtern ? "extern " : "")
            .Append(IsOverride ? "override " : "")
            .Append(IsSealed ? "sealed " : "")
            .Append(IsStatic ? "static " : "")
            .Append(ReturnType).Append(' ')
            .Append(Name).Append('(')
            .Append(string.Join(", ", parameters)).Append(')')
            .AppendLine("{")
            .AppendLine(Body.ToString())
            .Append('}')
            .ToString();
    }
}
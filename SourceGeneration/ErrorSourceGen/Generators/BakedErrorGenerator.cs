using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using ErrorSourceGen.Builders;

using Microsoft.CodeAnalysis;

using Accessibility = ErrorSourceGen.Builders.Accessibility;

namespace ErrorSourceGen.Generators;

public class BakedErrorGenerator : ErrorGenerator
{
    #nullable enable
    private ErrorsContract? Contract { get; set; }
    #nullable disable

    public override bool CanHandleManifest(string path)
    {
        return Regex.IsMatch(path, @".*\.Resources\.BakedError\..*\.json");
    }

    public override void InitJson(string json)
    {
        Contract = Deserialize<ErrorsContract>(json);
    }

    public override void Flush(GeneratorExecutionContext context)
    {
        if (Contract == null)
            return;
        
        // Take note of https://stackoverflow.com/a/184975/16324801

        var className = "public partial record struct BakedError(" +
                        "string Id, string Name, " +
                        "string ShortDescription, string LongDescription, " +
                        "ulong SourceIndex)";
        
        context.AddSource("BakedError.g.cs", 
            $@"#nullable enable
namespace BakedEnv
{{
{className}
{{
    {string.Join(Environment.NewLine, Contract.Properties.Select(CreateErrorMethod))}
}}
}}");
    }

    private string CreateErrorMethod(KeyValuePair<string, ErrorContract> contract)
    {
        var shortFormatTags = FindFormatTags(contract.Value.ShortDescription);
        var longFormatTags = FindFormatTags(contract.Value.LongDescription);
        var shortFormatParams = shortFormatTags.Select(f => ($"@{f}", "object")).ToList();
        var longFormatParams = longFormatTags.Select(f => ($"@{f}", "object")).ToList();

        var builder = new MethodBuilder()
            .WithName($"E{contract.Value.Name}")
            .WithParameters(shortFormatParams.Concat(longFormatParams))
            .WithParameter("sourceIndex", "ulong")
            .AsStatic()
            .WithAccessibility(Accessibility.Public)
            .WithReturnType("BakedError");

        var shortFormatString = shortFormatTags.Count != 0 ? $", {string.Join(", ", shortFormatTags)}" : "";
        var longFormatString = longFormatTags.Count != 0 ? $", {string.Join(", ", longFormatTags)}" : "";
        
        builder.Body
            .AppendLine("return new BakedError(")
            .AppendLine($"\"{contract.Key}\",")
            .AppendLine($"\"{contract.Value.Name}\",")
            .AppendLine($"string.Format(\"{contract.Value.ShortDescription}\"{shortFormatString}),")
            .AppendLine($"string.Format(\"{contract.Value.LongDescription}\"{longFormatString}),")
            .AppendLine("sourceIndex);");

        builder.XmlDoc.Name = "summary";
        builder.XmlDoc.Value = new StringBuilder()
            .Append("ID: ").Append(contract.Key).Append("<br/>")
            .Append(contract.Value.ShortDescription).Append("<br/><br/>")
            .Append(contract.Value.LongDescription)
            .ToString();

        return builder.ToString();
    }

    #nullable enable
    private HashSet<string> FindFormatTags(params string?[] strings)
    {
        var set = new HashSet<string>();
        var regex = new Regex(@"(?<!\\)\{[\w_][_\w\d]*\}");
        // https://regex101.com/r/etBhAP/1

        foreach (var s in strings.OfType<string>())
        {
            foreach (Match match in regex.Matches(s))
            {
                var trimmed = match.Value.Substring(1, match.Value.Length - 2);

                set.Add(trimmed);
            }
        }

        return set;
    }
}
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;

namespace ErrorSourceGen;

public class ErrorClassGenerator
{
    #nullable enable
    public ErrorsContract? Contract { get; set; }
    #nullable disable
    
    public void Flush(GeneratorExecutionContext context)
    {
        if (Contract == null)
            return;
        
        // Take note of https://stackoverflow.com/a/184975/16324801
        
        context.AddSource("BakedError.g.cs", 
            $@"#nullable enable
namespace BakedEnv
{{
public partial record struct BakedError(string? Id, string Name, string ShortDescription, string LongDescription, ulong SourceIndex)
{{
    {string.Join(Environment.NewLine, Contract.Properties.Select(CreateErrorMethod))}
}}
}}");
    }

    private string CreateErrorMethod(KeyValuePair<string, ErrorContract> contract)
    {
        var formatTags = FindFormatTags(
            contract.Value.ShortDescription, contract.Value.LongDescription);
        var formatParams = formatTags.Select(f => $"object? @{f}").ToList();
        
        formatParams.Add("ulong sourceIndex");

        return $@"
/// <summary>
/// <b>{contract.Value.Name}</b> ({contract.Key}) <br/>
/// {contract.Value.ShortDescription}
/// </summary>
public static BakedError E{contract.Value.Name}({string.Join(",", formatParams)})
{{
    return new BakedError(
        ""{contract.Key}"", 
        ""{contract.Value.Name}"", 
        ""{contract.Value.ShortDescription}"", 
        ""{contract.Value.LongDescription}"",
        sourceIndex);
}}";
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
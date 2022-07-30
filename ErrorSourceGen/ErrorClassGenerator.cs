using System.Text;
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
        
        context.AddSource("BakedError.g.cs", 
            $@"#nullable enable
public partial record struct BakedError(string? Id, string Name, string ShortDescription, string LongDescription, int SourceIndex)
{{
    {string.Join(Environment.NewLine, Contract.Select(CreateErrorGroupClass))}
}}");
    }

    private string CreateErrorGroupClass(KeyValuePair<string, ErrorGroupContract> contract)
    {
        return $@"
public static class {contract.Key}
{{
    {string.Join(Environment.NewLine, contract.Value.Select(CreateErrorMethod))}
}}";
    }

    private string CreateErrorMethod(KeyValuePair<string, ErrorContract> contract)
    {
        var formatTags = FindFormatTags(
            contract.Value.ShortDescription, contract.Value.LongDescription);
        var formatParams = formatTags.Select(f => $"object? {f}").ToList();
        
        formatParams.Add("int sourceIndex");

        return $@"
/// <summary>
/// <b>{contract.Value.Name}</b> <br/>
/// {contract.Value.ShortDescription}
/// </summary>
public static BakedError E{contract.Key}({string.Join(",", formatParams)})
{{
    return new BakedError(
        ""{contract.Key}"", 
        ""{contract.Value.Name}"", 
        ""{contract.Value.ShortDescription}"", 
        ""{contract.Value.ShortDescription}"",
        sourceIndex);
}}";
    } // Return BakedError instead

    private HashSet<string> FindFormatTags(params string[] strings)
    {
        var set = new HashSet<string>();
        var regex = new Regex(@"(?<!\\)\{[\w_][_\w\d]*\}");
        // https://regex101.com/r/4Wj3u9/1

        foreach (var s in strings)
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
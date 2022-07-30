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
            $@"
public partial class BakedError
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
        return $@"
/// <summary>
/// <b>{contract.Value.Name}</b> <br/>
/// {contract.Value.ShortDescription}
/// </summary>
public static string E{contract.Key}()
{{
    return ""{contract.Value.ShortDescription}"";
}}";
    }
}
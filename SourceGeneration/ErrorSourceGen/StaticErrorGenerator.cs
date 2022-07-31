using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.CodeAnalysis;

namespace ErrorSourceGen;

[Generator]
public class StaticErrorsGenerator : ISourceGenerator
{
    private ErrorClassGenerator ClassGenerator { get; set; }
    private OutputGenerator Output { get; set; }
    
    public void Initialize(GeneratorInitializationContext context)
    {
        ClassGenerator = new ErrorClassGenerator();
        Output = new OutputGenerator();
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames();
        
        foreach (var name in resourceNames)
        {
            if (name.StartsWith($"{assembly.GetName().Name}.Resources"))
            {
                HandleJson(assembly, name);
            }
        }
        
        ClassGenerator.Flush(context);
        Output.Flush(context);
    }

    private void HandleJson(Assembly assembly, string manifest)
    {
        using var manifestStream = assembly.GetManifestResourceStream(manifest);
        
        if (manifestStream == null)
            return;
        
        var reader = new StreamReader(manifestStream);
        
        if (JsonSerializer.Deserialize<ErrorsContract>(reader.ReadToEnd()) is not { } result)
            return;

        ClassGenerator.Contract = result;
    }
}
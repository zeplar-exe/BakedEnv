using System.Diagnostics;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace ErrorSourceGen;

[Generator]
public class StaticErrorsGenerator : ISourceGenerator
{
    private OutputGenerator Output { get; set; }
    
    public void Initialize(GeneratorInitializationContext context)
    {
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
                HandleJson(name);
            }
        }
        
        Output.Flush(context);
    }

    private void HandleJson(string manifest)
    {
        Output.WriteLine(manifest);
    }
}
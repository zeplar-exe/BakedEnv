using System.Reflection;
using Microsoft.CodeAnalysis;

namespace ErrorSourceGen;

[Generator]
public class StaticErrorsGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        throw new Exception(string.Join(", ", resourceNames));
    }
}
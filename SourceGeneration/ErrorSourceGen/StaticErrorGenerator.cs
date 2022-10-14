using System.Reflection;

using ErrorSourceGen.Common;
using ErrorSourceGen.Generators;

using Microsoft.CodeAnalysis;

namespace ErrorSourceGen;

[Generator]
public class StaticErrorsGenerator : ISourceGenerator
{
    private GenericTypeList<ErrorGenerator> Generators { get; }
    private OutputGenerator Output { get; }

    public StaticErrorsGenerator()
    {
        Generators = new GenericTypeList<ErrorGenerator>();
        Output = new OutputGenerator();
        
        Generators.Add<BakedErrorGenerator>();
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
        
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames();
        
        foreach (var name in resourceNames)
        {
            foreach (var generator in Generators)
            {
                if (generator.CanHandleManifest(name))
                {
                    using var stream = assembly.GetManifestResourceStream(name);
                    
                    if (stream == null)
                        continue;

                    using var reader = new StreamReader(stream);
                    
                    generator.InitJson(reader.ReadToEnd());
                    generator.Flush(context);
                }
            }
        }
        
        Output.Flush(context);
    }
}
using System.Text;

using Microsoft.CodeAnalysis;

namespace ErrorSourceGen;

public class OutputGenerator
{
    private StringBuilder Builder { get; }

    public OutputGenerator()
    {
        Builder = new StringBuilder();
    }
    
    public void Write(object text)
    {
        Builder.Append(text);
    }

    public void WriteLine(object text)
    {
        Builder.AppendLine(text.ToString());
    }

    public void Flush(GeneratorExecutionContext context)
    {
        context.AddSource("GeneratedOutput.g.cs", 
            $"public static class GeneratedOutput {{ public static string Text = @\"{Builder}\"; }}");
    }
}
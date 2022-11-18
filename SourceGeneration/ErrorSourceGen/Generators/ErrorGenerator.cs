using System.Text.Json;

using Microsoft.CodeAnalysis;

namespace ErrorSourceGen.Generators;

public abstract class ErrorGenerator
{
    public abstract bool CanHandleManifest(string path);
    public abstract void InitJson(string json);
    public abstract void Flush(GeneratorExecutionContext context);

    protected T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }
}
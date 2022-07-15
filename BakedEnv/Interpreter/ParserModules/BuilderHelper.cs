using System.Diagnostics.CodeAnalysis;

namespace BakedEnv.Interpreter.ParserModules;

internal class BuilderHelper
{
    public static void EnsurePropertyNotNull([NotNull] object? o)
    {
        if (o == null)
        {
            throw new InvalidOperationException($"Cannot build with an invalid state ({nameof(o)} is null).");
        }
    }
}
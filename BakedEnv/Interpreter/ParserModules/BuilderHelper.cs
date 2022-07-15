using System.Diagnostics.CodeAnalysis;
using TokenCs;

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
    
    public static void EnsureLexerToken([NotNull] LexerToken? token, LexerTokenType expectedType)
    {
        if (token == null)
        {
            throw new InvalidOperationException(
                $"Cannot build with an invalid state ({nameof(token)} is null).");
        }

        if (token.Type != expectedType)
        {
            throw new InvalidOperationException(
                $"Cannot build with an invalid state {nameof(token)} is not of type '{expectedType}'.");
        }
    }
}
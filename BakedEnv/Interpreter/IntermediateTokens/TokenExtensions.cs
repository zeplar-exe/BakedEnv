using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateTokens;

internal static class TokenExtensions
{
    public static T AsComplete<T>(this T token, bool complete = true) where T : IntermediateToken
    {
        token.IsComplete = complete;

        return token;
    }

    public static T AsIncomplete<T>(this T token) where T : IntermediateToken
    {
        token.IsComplete = false;

        return token;
    }

    public static bool IsRawType(this IntermediateToken token, TextualTokenType type)
    {
        return token is RawIntermediateToken raw && raw.Type == type;
    }
}
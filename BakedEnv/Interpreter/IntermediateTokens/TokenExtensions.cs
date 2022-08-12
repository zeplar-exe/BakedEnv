using BakedEnv.Interpreter.IntermediateTokens.Pure;

namespace BakedEnv.Interpreter.IntermediateTokens;

internal static class TokenExtensions
{
    public static T AsComplete<T>(this T token, bool complete = true) where T : PureIntermediateToken
    {
        token.IsComplete = complete;

        return token;
    }

    public static T AsIncomplete<T>(this T token) where T : PureIntermediateToken
    {
        token.IsComplete = false;

        return token;
    }
}
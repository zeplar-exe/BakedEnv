using System.Diagnostics.CodeAnalysis;

namespace BakedEnv.GeneralTests;

public static class TestHelper
{
    public static bool ObjectIs<T>(object? o, [NotNullWhen(true)] out T? t)
    {
        t = default;

        if (o is T converted)
        {
            t = converted;

            return true;
        }

        return false;
    }
}
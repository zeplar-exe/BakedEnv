using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace BakedEnv.GeneralTests;

public class TestHelper
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
    
    public static T AssertIsType<T>(object? o)
    {
        Assert.IsInstanceOf<T>(o);

        return (T)o!;
    }
}
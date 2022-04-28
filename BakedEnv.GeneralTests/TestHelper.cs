using NUnit.Framework;

namespace BakedEnv.GeneralTests;

public class TestHelper
{
    public static T AssertIsType<T>(object? o)
    {
        Assert.IsInstanceOf<T>(o);

        return (T)o!;
    }
}
using BakedEnv;

namespace SourceGeneration.Tests.ErrorSourceGen;

public class BaseTests
{
    [Test]
    public void TestBakedError()
    {
        Console.WriteLine(BakedError.EInvalidLocalVariable("abc123", 0));
    }
}
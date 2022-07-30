using BakedEnv.Interpreter;

namespace SourceGeneration.Tests.ErrorSourceGen;

public class BaseTests
{
    [Test]
    public void TestBakedError()
    {
        Console.WriteLine(BakedError.VAR.E1000("abc123", 0));
    }
}
using BakedEnv.ExternalApi;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.ApiStructureTests;

[TestFixture]
public class ObjectStructure
{
    [Test]
    public void TestBasicObject()
    {
        var structure = ApiStructure.FromObject(new BasicApiStructureObject());

        var targetValue = structure.Root.GetProperty("Foo")?.Value.Value;
        
        Assert.True(targetValue?.ToString() == "Bar");
    }
}

public class BasicApiStructureObject
{
    public string Foo { get; }

    public BasicApiStructureObject()
    {
        Foo = "Bar";
    }
}
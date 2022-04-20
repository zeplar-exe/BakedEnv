using System.IO;
using BakedEnv.ExternalApi;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.ApiStructureTests;

[TestFixture]
public class XmlStructure
{
    [Test]
    public void TestXmlInput()
    {
        var xml = TestFileHelper.ReadXml("ApiStructureXmlTest1.xml");
        var structure = ApiStructure.FromXml(xml.Root!);

        var targetValue = structure.Root
            .GetProperty("MyProperty")?.Value
            .GetProperty("MyNestedProperty")?.Value
            .Value;

        Assert.True(targetValue?.ToString() == "Hello world!");
    }

    [Test]
    public void TestXmlFileInput()
    {
        var file = TestFileHelper.GetRelativeFilePath("ApiStructureXmlTest1.xml");
        var structure = ApiStructure.FromXml(file);
        
        var targetValue = structure.Root
            .GetProperty("MyProperty")?.Value
            .GetProperty("MyNestedProperty")?.Value
            .Value;

        Assert.True(targetValue?.ToString() == "Hello world!");
    }

    [Test]
    public void TestInvalidXmlFileInput()
    {
        var file = TestFileHelper.GetRelativeFilePath("IDoNotExist-123.xml");

        Assert.Catch<FileNotFoundException>(delegate
        {
            _ = ApiStructure.FromXml(file);
        });
    }
}
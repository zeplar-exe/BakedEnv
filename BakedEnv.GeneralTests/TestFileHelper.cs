using System.IO;
using System.Xml.Linq;

namespace BakedEnv.GeneralTests;

public static class TestFileHelper
{
    public static string GetRelativeFilePath(string relative)
    {
        return Path.Join(Directory.GetCurrentDirectory(), "TestFiles", relative);
    }
    
    public static Stream CreateStream(string relative)
    {
        return File.OpenRead(GetRelativeFilePath(relative));
    }
    
    public static string ReadRaw(string relative)
    {
        var path = GetRelativeFilePath(relative);

        return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
    }
    
    public static XDocument ReadXml(string relative)
    {
        return XDocument.Parse(ReadRaw(relative));
    }
}
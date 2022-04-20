using System.IO;
using System.Linq;
using System.Text;
using BakedEnv.Interpreter.Sources;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class BakedSourceImplementations
{
    [Test]
    public void TestStreamSource()
    {
        const string TestString = "Hello world! I am foo, and this is my friend bar.";
        
        var utf8Source = new StreamSource(CreateStream(TestString), Encoding.UTF8);
        var utf32Source = new StreamSource(CreateStream(TestString), Encoding.UTF32);
        var asciiSource = new StreamSource(CreateStream(TestString), Encoding.ASCII);

        var utf8Text = string.Concat(utf8Source.EnumerateCharacters());
        var utf32Text = string.Concat(utf32Source.EnumerateCharacters());
        var asciiText = string.Concat(asciiSource.EnumerateCharacters());
        
        Assert.True(AllEqual(utf8Text, utf32Text, asciiText, TestString));
    }
    
    private bool AllEqual<T>(params T[] values) {
        if (values.Length == 0)
            return true;
        
        return values.All(v => v.Equals(values[0]));    
    }
    
    private static Stream CreateStream(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        
        writer.Write(s);
        writer.Flush();
        
        stream.Position = 0;
        
        return stream;
    }
}
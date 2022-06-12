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
        var fullText = TestFileHelper.ReadRaw("source_test.txt");
        var utf32Source = new StreamSource(TestFileHelper.CreateStream("source_test.txt"), Encoding.UTF32);
        
        var utf32Text = string.Concat(utf32Source.EnumerateCharacters());
        
        Assert.True(AllEqual(utf32Text, fullText));
    }

    [Test]
    public void TestFileSource()
    {
        var fullText = TestFileHelper.ReadRaw("source_test.txt");
        var utf32File = new FileSource(TestFileHelper.GetRelativeFilePath("source_test.txt"), Encoding.UTF32);
        var utf32Text = string.Concat(utf32File.EnumerateCharacters());
        
        Assert.True(AllEqual(utf32Text, fullText));
    }
    
    private bool AllEqual<T>(params T[] values) {
        if (values.Length == 0)
            return true;
        
        return values.All(v => v?.Equals(values[0]) ?? false);    
    }
}
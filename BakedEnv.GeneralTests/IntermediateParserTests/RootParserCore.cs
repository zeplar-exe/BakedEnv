using System.Linq;

using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateTokens;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

[TestFixture]
public class RootParserCore
{
    [Test]
    public void EmptyInputIsEndOfFile()
    {
        var parser = new RootParser();
        var iterator = LexerHelper.CreateIterator(string.Empty);

        var resultEnumerable = parser.Parse(iterator);
        var last = resultEnumerable.Last();
        
        Assert.That(last, Is.TypeOf<EndOfFileToken>());
    }
    
    [Test]
    public void EndOfFileIsCreated()
    {
        var parser = new RootParser();
        var iterator = LexerHelper.CreateIterator("abc");

        var resultEnumerable = parser.Parse(iterator);
        var last = resultEnumerable.Last();
        
        Assert.That(last, Is.TypeOf<EndOfFileToken>());
    }
}
using System.Linq;

using BakedEnv.Interpreter.IntermediateTokens.Pure;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

[TestFixture]
public class StringParserCases
{
    [Test]
    public void EmptyStringIsValid()
    {
        AssertStringTokenIsEqual("");
    }

    [Test]
    public void RegularStringIsValid()
    {
        AssertStringTokenIsEqual("Hello world! My name is Mark");
    }

    [Test]
    public void EscapeQuotationIsValid()
    {
        AssertStringTokenIsEqual("\\\"");
    }

    [Test]
    public void EscapeInvalidCodeIsRawCharacter()
    {
        AssertStringTokenIsEqual("\\j paw");
    }

    [Test]
    public void NewlineIsValid()
    {
        AssertStringTokenIsEqual("\n");
        AssertStringTokenIsEqual("\r");
        AssertStringTokenIsEqual("\r\n");
    }

    [Test]
    public void IncompleteStringIsFailure()
    {
        Assert.That(ParserHelper.TryGetFirst("\"abc123", out var token), Is.True);
        Assert.That(token.IsComplete, Is.False);
    }
    
    private void AssertStringTokenIsEqual(string content)
    {
        var token = ParserHelper.AssertFirstIs<StringToken>($"\"{content}\"");
        var join = string.Concat(token.Content.Select(t => t.RawToken.ToString()));
        
        Assert.That(join, Is.EqualTo(content));
    }
}
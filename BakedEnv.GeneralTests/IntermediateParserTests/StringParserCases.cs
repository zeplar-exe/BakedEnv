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
        AssertStringTokenContentSame("");
    }

    [Test]
    public void RegularStringIsValid()
    {
        AssertStringTokenContentSame("Hello world! My name is Mark");
    }

    [Test]
    public void EscapeQuotationIsValid()
    {
        AssertStringTokenContentSame("\\\"");
    }

    [Test]
    public void EscapeInvalidCodeIsRawCharacter()
    {
        AssertStringTokenContentSame("\\j paw");
    }

    [Test]
    public void IncompleteStringIsFailure()
    {
        Assert.That(ParserHelper.TryGetFirst("\"abc123", out var token), Is.True);
        Assert.That(token.IsComplete, Is.False);
    }
    
    private void AssertStringTokenContentSame(string content)
    {
        var token = AssertFirstIsString(content);
        var join = string.Concat(token.Content.Select(t => t.RawToken.ToString()));
        
        Assert.That(join, Is.EqualTo(content));
    }

    private StringToken AssertFirstIsString(string input)
    {
        Assert.That(ParserHelper.TryGetFirst($"\"{input}\"", out var token), Is.True);
        Assert.That(token, Is.TypeOf<StringToken>());

        return (StringToken)token!;
    }
}
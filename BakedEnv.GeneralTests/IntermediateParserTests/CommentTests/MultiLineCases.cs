using BakedEnv.Interpreter.IntermediateTokens;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests.CommentTests;

[TestFixture]
public class MultiLineCases
{
    [Test]
    public void MultiLineIsValid()
    {
        AssertMultiLineCommentContentIsEqual("abc 123 \n abc1234");
    }

    [Test]
    public void EmptyMultiLineIsValid()
    {
        AssertMultiLineCommentIsComplete("####", true);
    }

    [Test]
    public void MultiLineWithSingleHashIsValid()
    {
        AssertMultiLineCommentContentIsEqual("abc 123 \n # abc1234");
    }

    [Test]
    public void MultiLineEndOfFileIsInvalid()
    {
        AssertMultiLineCommentIsComplete("## abc123", false);
    }

    [Test]
    public void MultiLineSingleLineIsValid()
    {
        AssertMultiLineCommentContentIsEqual("abc123");
    }

    private void AssertMultiLineCommentIsComplete(string input, bool complete)
    {
        var token = ParserHelper.AssertFirstIs<MultiLineCommentToken>(input);
        
        Assert.That(token.IsComplete, Is.EqualTo(complete));
    }

    private void AssertMultiLineCommentContentIsEqual(string input)
    {
        var token = ParserHelper.AssertFirstIs<MultiLineCommentToken>($"##{input}##");
        
        Assert.That(token.IsComplete, Is.True);
        
        var concat = string.Concat(token.Content);
        
        Assert.That(concat, Is.EqualTo(input));
    }
}
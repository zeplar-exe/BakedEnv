using BakedEnv.Interpreter.IntermediateTokens.Pure;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

[TestFixture]
public class CommentParserCases
{
    [Test]
    public void SingleLineIsValid()
    {
        AssertSingleLineCommentIsComplete("# abc\n");
    }

    [Test]
    public void SingleLineEndOfFileIsValid()
    {
        AssertSingleLineCommentIsComplete("# abc");
    }

    [Test]
    public void EmptySingleLineIsValid()
    {
        AssertSingleLineCommentIsComplete("#");
    }

    [Test]
    public void MultiLineIsValid()
    {
        AssertMultiLineCommentIsComplete("## abc 123\nabc1234 ##", true);
    }

    [Test]
    public void MultiLineEndOfFileIsInvalid()
    {
        AssertMultiLineCommentIsComplete("## abc123", false);
    }

    [Test]
    public void MultiLineSingleLineIsValid()
    {
        AssertMultiLineCommentIsComplete("## abc123 ##", true);
    }

    private void AssertSingleLineCommentIsComplete(string input)
    {
        var token = ParserHelper.AssertFirstIs<SingleLineCommentToken>(input);
        
        Assert.That(token.IsComplete, Is.True);
    }

    private void AssertMultiLineCommentIsComplete(string input, bool complete)
    {
        var token = ParserHelper.AssertFirstIs<MultiLineCommentToken>(input);
        
        Assert.That(token.IsComplete, Is.EqualTo(complete));
    }
}
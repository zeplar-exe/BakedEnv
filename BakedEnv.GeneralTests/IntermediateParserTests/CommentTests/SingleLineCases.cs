using BakedEnv.Interpreter.IntermediateTokens;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests.CommentTests;

[TestFixture]
public class SingleLineCases
{
    [Test]
    public void SingleLineIsValid()
    {
        AssertSingleLineCommentContentIsEqual(" abc\n");
    }

    [Test]
    public void SingleLineEndOfFileIsValid()
    {
        AssertSingleLineCommentContentIsEqual(" abc");
    }

    [Test]
    public void SingleLineWithHashIsValid()
    {
        AssertSingleLineCommentContentIsEqual(" abc #");
    }

    [Test]
    public void EmptySingleLineIsValid()
    {
        AssertSingleLineCommentIsComplete("#");
    }
    
    private void AssertSingleLineCommentIsComplete(string input)
    {
        var token = ParserHelper.AssertFirstIs<SingleLineCommentToken>(input);
        
        Assert.That(token.IsComplete, Is.True);
    }
    
    private void AssertSingleLineCommentContentIsEqual(string input)
    {
        var token = ParserHelper.AssertFirstIs<SingleLineCommentToken>($"#{input}");
        
        Assert.That(token.IsComplete, Is.True);
        
        var concat = string.Concat(token.Content);
        
        Assert.That(concat, Is.EqualTo(input));
    }
}
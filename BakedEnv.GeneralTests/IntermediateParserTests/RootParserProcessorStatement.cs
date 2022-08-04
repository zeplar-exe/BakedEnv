using BakedEnv.Interpreter.IntermediateTokens.Pure;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

[TestFixture]
public class RootParserProcessorStatement
{
    [Test]
    public void ValidProcessorStatementIsSuccess()
    {
        Assert.That(ParserHelper.TryGetFirst("[]", out var token), Is.True);
        Assert.That(token, Is.TypeOf<ProcessorStatementToken>());
        Assert.That(token.IsComplete, Is.True);
    }

    [Test]
    public void ValidProcessorStatementWithTriviaIsSuccess()
    {
        Assert.That(ParserHelper.TryGetFirst("[ \n\r\t ]", out var token), Is.True);
        Assert.That(token, Is.TypeOf<ProcessorStatementToken>());
        Assert.That(token.IsComplete, Is.True);
    }
}
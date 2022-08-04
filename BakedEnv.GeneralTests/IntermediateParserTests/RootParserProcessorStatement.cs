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
    }

    [Test]
    public void IncompleteProcessorStatementIsFailure()
    {
        Assert.That(ParserHelper.TryGetFirst("][", out var token), Is.True);
        Assert.That(token.IsComplete, Is.False);
    }
}
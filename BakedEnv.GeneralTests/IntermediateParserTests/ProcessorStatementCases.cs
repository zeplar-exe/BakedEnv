using BakedEnv.Interpreter.IntermediateTokens.Pure;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.IntermediateParserTests;

[TestFixture]
public class ProcessorStatementCases
{
    [Test]
    public void ValidProcessorStatementIsSuccess()
    {
        AssertStatementTokenIsComplete("[]");
    }

    [Test]
    public void ValidProcessorStatementWithTriviaIsSuccess()
    {
        AssertStatementTokenIsComplete("[ \n\r\t ]");
    }

    [Test]
    public void IncompleteProcessorStatementIsFailure()
    {
        var token = AssertInputIsProcessorStatement("[ abc");
        Assert.That(token.IsComplete, Is.False);
    }

    private void AssertStatementTokenIsComplete(string input)
    {
        var token = AssertInputIsProcessorStatement(input);
        Assert.That(token.IsComplete, Is.True);
    }

    private ProcessorStatementToken AssertInputIsProcessorStatement(string input)
    {
        Assert.That(ParserHelper.TryGetFirst(input, out var token), Is.True);
        Assert.That(token, Is.TypeOf<ProcessorStatementToken>());

        return (ProcessorStatementToken)token!;
    }
}
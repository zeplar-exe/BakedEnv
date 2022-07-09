using BakedEnv.Interpreter.Sources;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Expressions
{
    [Test]
    public void TestParenthesisNumber()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = ((1))")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(session.Interpreter.Context.Variables["foo"].Value.Equals(1));
    }
}
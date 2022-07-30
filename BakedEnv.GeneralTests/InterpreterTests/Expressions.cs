using BakedEnv.Environment;
using BakedEnv.Sources;

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
        
        session.AssertInterpreterHasVariable("foo", 1);
    }
}
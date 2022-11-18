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
        var session = InterpreterTestHelper.CreateSession("foo = ((1))");
        session.ExecuteUntilEnd();
        
        session.AssertInterpreterHasVariable("foo", 1);
    }
}
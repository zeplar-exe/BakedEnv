using System;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Expressions
{
    [Test]
    public void TestParenthesisNumber()
    {
        var session = InterpreterTestHelper.CreateSession("foo = ((1))");
        session.ExecuteUntilError();
        
        session.AssertInterpreterHasVariable("foo", 1);
    }
}
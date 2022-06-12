using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Variables
{
    [Test]
    public void TestVariableAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("pizza = 1")).Init();
        session.ExecuteUntilEnd();

        Assert.True(session.Interpreter.Context.Variables["pizza"].Equals(1));
    }

    [Test]
    public void TestMultipleVariableAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = 1 \n bar=2")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(session.Interpreter.Context.Variables["bar"].Equals(2));
    }

    [Test]
    public void TestMultiAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = 1 \n foo=2")).Init();
        session.ExecuteUntilEnd();

        Assert.True(session.Interpreter.Context.Variables["foo"].Equals(2));
    }
    
    [Test]
    public void TestReadOnlyVariable()
    {
        var environment = new BakedEnvironment().WithReadOnlyVariable("Foo", new BakedInteger(1));
        var session = environment.CreateSession(new RawStringSource("Foo = 0")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(environment.ReadOnlyGlobalVariables["Foo"].Equals(1));
    }
}
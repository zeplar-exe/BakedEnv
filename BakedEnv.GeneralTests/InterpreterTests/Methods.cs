using System;
using System.Numerics;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Methods
{
    [Test]
    public void TestMethod()
    {
        var target = 0;
        var del = new DelegateObject(delegate() { target = 1; });

        var environment = new BakedEnvironment().WithVariable("foo", del);
        var session = environment.CreateSession(new RawStringSource("foo()")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(target == 1);
    }

    [Test]
    public void TestParameterizedMethod()
    {
        var target = BigInteger.Zero;
        var del = new DelegateObject(delegate(BigInteger i) { target = i; });

        var environment = new BakedEnvironment().WithVariable("foo", del);
        var session = environment.CreateSession(new RawStringSource("foo(5)")).Init();
        session.ExecuteUntilEnd();

        Assert.True(target == 5);
    }

    [Test]
    public void TestMethodReturn()
    {
        var del = new DelegateObject(delegate() { return "Hello world!"; });
        
        var environment = new BakedEnvironment().WithVariable("foo", del);
        var session = environment.CreateSession(new RawStringSource("a = foo()")).Init();
        session.ExecuteUntilEnd();

        Assert.True(session.TopVariables["a"].Value.Equals("Hello world!"));
    }
}
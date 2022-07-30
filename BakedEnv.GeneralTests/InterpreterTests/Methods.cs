using System.Numerics;

using BakedEnv.Environment;
using BakedEnv.Objects;
using BakedEnv.Sources;

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

        var environment = new BakedEnvironmentBuilder().WithVariable("foo", del).Build();
        var session = environment.CreateSession(new RawStringSource("foo()")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(target == 1);
    }

    [Test]
    public void TestParameterizedMethod()
    {
        var target = BigInteger.Zero;
        var del = new DelegateObject(delegate(BigInteger i) { target = i; });
        
        // TODO: Right parenthesis after number is ignored? (ParserTools Lexer)

        var environment = new BakedEnvironmentBuilder().WithVariable("foo", del).Build();
        var session = environment.CreateSession(new RawStringSource("foo(5)")).Init();
        session.ExecuteUntilEnd();

        Assert.True(target == 5);
    }

    [Test]
    public void TestMethodReturn()
    {
        var del = new DelegateObject(delegate() { return "Hello world!"; });
        
        var environment = new BakedEnvironmentBuilder().WithVariable("foo", del).Build();
        var session = environment.CreateSession(new RawStringSource("a = foo()")).Init();
        session.ExecuteUntilEnd();

        session.AssertInterpreterHasVariable("a", "Hello world!");
    }
}
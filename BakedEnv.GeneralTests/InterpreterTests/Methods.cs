using System.Numerics;

using BakedEnv.Environment;
using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Methods
{
    [Test]
    public void TestMethod()
    {
        var target = 0;
        var del = new DelegateObject(delegate() { target = 1; }, MappedConversionTable.Primitive());

        var environment = new BakedEnvironmentBuilder().WithVariable("foo", del).Build();
        var session = environment.CreateSession("foo()");
        session.ExecuteUntilError();
        
        Assert.True(target == 1);
    }

    [Test]
    public void TestParameterizedMethod()
    {
        var target = BigInteger.Zero;
        var del = new DelegateObject(delegate(BigInteger i) { target = i; }, MappedConversionTable.Primitive());

        var environment = new BakedEnvironmentBuilder().WithVariable("foo", del).Build();
        var session = environment.CreateSession("foo(5)");
        session.ExecuteUntilError();

        Assert.True(target == 5);
    }

    [Test]
    public void TestMethodReturn()
    {
        var del = new DelegateObject(delegate() { return "Hello world!"; }, MappedConversionTable.Primitive());
        
        var environment = new BakedEnvironmentBuilder().WithVariable("foo", del).Build();
        var session = environment.CreateSession("a = foo()");
        session.ExecuteUntilError();

        session.AssertInterpreterHasVariable("a", "Hello world!");
    }
}
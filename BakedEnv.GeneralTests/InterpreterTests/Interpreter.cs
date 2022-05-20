using System;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Sources;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Interpreter
{
    [Test]
    public void TestPreparation()
    {
        var interpreter = InitInterpreter(new RawStringSource("Hello world!"));
        
        Assert.True(interpreter.TryGetNextInstruction(out _));
    }

    [Test]
    public void TestSourcelessPreparation()
    {
        var interpreter = new BakedInterpreter();

        Assert.Catch<InvalidOperationException>(delegate
        {
            interpreter.Init();
        });
    }

    [Test]
    public void TestVariableAssignment()
    {
        var environment = new BakedEnvironment();

        environment.WithExitHandlers(new ExitHandler(delegate(BakedInterpreter interpreter)
        {
            Assert.True(interpreter.Context.Variables["pizza"].Equals(1));
        }));
        
        environment.Invoke(new RawStringSource("pizza = 1"));
    }

    [Test]
    public void TestMultipleVariableAssignment()
    {
        var environment = new BakedEnvironment();

        environment.WithExitHandlers(new ExitHandler(delegate(BakedInterpreter interpreter)
        {
            Assert.True(interpreter.Context.Variables["bar"].Equals(2));
        }));
        
        environment.Invoke(new RawStringSource("foo = 1 \n bar=2"));
    }

    [Test]
    public void TestMultiAssignment()
    {
        var environment = new BakedEnvironment();

        environment.WithExitHandlers(new ExitHandler(delegate(BakedInterpreter interpreter)
        {
            Assert.True(interpreter.Context.Variables["foo"].Equals(2));
        }));
        
        environment.Invoke(new RawStringSource("foo = 1 \n foo=2"));
    }

    private BakedInterpreter InitInterpreter(IBakedSource source)
    {
        var interpreter = new BakedInterpreter()
            .WithSource(source);
        
        interpreter.Init();

        return interpreter;
    }
}
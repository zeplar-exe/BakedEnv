using System;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Interpreter
{
    [Test]
    public void TestPreparation()
    {
        var source = new RawStringSource("Hello world!");
        var interpreter = new BakedInterpreter()
            .WithSource(source);
        
        interpreter.Init();
        
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
        var source = new RawStringSource("pizza = 1");
        var interpreter = new BakedInterpreter()
            .WithSource(source);
        
        interpreter.Init();
        
        if (!interpreter.TryGetNextInstruction(out var instruction))
            Assert.Fail();
        
        instruction.Execute(interpreter);
        
        Assert.True(interpreter.Context.Variables["pizza"].Equals(1));
    }

    [Test]
    public void TestMultipleVariableAssignment()
    {
        var source = new RawStringSource("foo = 1 \n bar=2");
        var interpreter = new BakedInterpreter()
            .WithSource(source);
        
        interpreter.Init();
        
        while (interpreter.TryGetNextInstruction(out var instruction))
            instruction.Execute(interpreter);
        
        Assert.True(interpreter.Context.Variables["bar"].Equals(2));
    }
}
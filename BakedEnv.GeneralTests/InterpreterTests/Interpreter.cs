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

    private BakedInterpreter InitInterpreter(IBakedSource source)
    {
        var interpreter = new BakedInterpreter()
            .WithSource(source);
        
        interpreter.Init();

        return interpreter;
    }
}
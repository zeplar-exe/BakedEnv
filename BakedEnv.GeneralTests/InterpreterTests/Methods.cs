using System;
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
        var testAction = new ActionInstruction((_, _) => target = 1);
        var testMethod = new BakedMethod(Array.Empty<string>());
        testMethod.Instructions.Add(testAction);

        var environment = new BakedEnvironment
        {
            GlobalVariables =
            {
                ["foo"] = testMethod
            }
        };
        
        var session = environment.CreateSession(new RawStringSource("foo()")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(target == 1);
    }
}
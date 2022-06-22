using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class ControlStatements
{
    [Test]
    public void TestIfStatementTrue()
    {
        var environment = new BakedEnvironment()
            .WithVariable("condition", new BakedBoolean(true))
            .WithVariable("target", new BakedInteger(0))
            .WithControlStatement("if", 1, new IfStatementExecution());
        var session = environment.CreateSession(new RawStringSource("if (condition) { target = 1 }")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(environment.GlobalVariables["target"].Value.Equals(1));
    }

    [Test]
    public void TestIfStatementFalse()
    {
        var environment = new BakedEnvironment()
            .WithVariable("condition", new BakedBoolean(false))
            .WithVariable("target", new BakedInteger(0))
            .WithControlStatement("if", 1, new IfStatementExecution());
        var session = environment.CreateSession(new RawStringSource("if (condition) { target = 1 }")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(environment.GlobalVariables["target"].Value.Equals(0));
    }
    
    private ScriptSession CreateSession(string text)
    {
        return new BakedEnvironment().CreateSession(new RawStringSource(text)).Init();
    }
}
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
        var environment = new BakedEnvironmentBuilder()
            .WithVariable("condition", new BakedBoolean(true))
            .WithVariable("target", new BakedInteger(0))
            .WithControlStatement(new IfStatementDefinition())
            .Build();
        var session = environment.CreateSession(new RawStringSource("if (condition) { target = 1 }")).Init();
        session.ExecuteUntilEnd();

        environment.AssertEnvironmentHasVariable("target", 1);
    }

    [Test]
    public void TestIfStatementFalse()
    {
        var environment = new BakedEnvironmentBuilder()
            .WithVariable("condition", new BakedBoolean(false))
            .WithVariable("target", new BakedInteger(0))
            .WithControlStatement(new IfStatementDefinition())
            .Build();
        var session = environment.CreateSession(new RawStringSource("if (condition) { target = 1 }")).Init();
        session.ExecuteUntilEnd();

        environment.AssertEnvironmentHasVariable("target", 0);
    }
}
using System;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Tables
{
    [Test]
    public void TestTableDeclaration()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = [ 0 : \"Bar\" ]")).Init();
        session.ExecuteUntilEnd();

        var obj = session.Interpreter.Context.Variables["foo"];
        var index = obj.Value.TryGetIndex(new BakedInteger(0), out var value);

        Assert.True(index && value.Equals("Bar"));
    }
}
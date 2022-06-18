using BakedEnv.Interpreter.Sources;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class VariableReferences
{
    [Test]
    public void TestGet()
    {
        var session = CreateSession("hello = \"world\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("hello", session.Interpreter);
        var value = reference.GetValue();
        
        Assert.True(value.Equals("world"));
    }
    
    [Test]
    public void TestGetInvalid()
    {
        var session = CreateSession("foo = \"bar\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("baz", session.Interpreter);
        var value = reference.GetValue();
        
        Assert.True(value is BakedNull);
    }

    private ScriptSession CreateSession(string text)
    {
        return new BakedEnvironment().CreateSession(new RawStringSource(text)).Init();
    }
}
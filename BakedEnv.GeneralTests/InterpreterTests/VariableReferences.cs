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
        if (!reference.TryGetVariable(out BakedVariable variable))
            Assert.Fail();
        
        Assert.True(variable.Value.Equals("world"));
    }
    
    [Test]
    public void TestGetInvalid()
    {
        var session = CreateSession("foo = \"bar\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("baz", session.Interpreter);
        reference.TryGetVariable(out BakedVariable variable);
        
        Assert.True(variable == null);
    }

    private ScriptSession CreateSession(string text)
    {
        return new BakedEnvironment().CreateSession(new RawStringSource(text)).Init();
    }
}
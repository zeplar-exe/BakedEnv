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
        
        Assert.True(reference.TryGetVariable(out var variable) && variable.Value.Equals("world"));
    }
    
    [Test]
    public void TestGetInvalid()
    {
        var session = CreateSession("foo = \"bar\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("baz", session.Interpreter);
        ;
        
        Assert.True(!reference.TryGetVariable(out BakedVariable variable) && variable == null);
    }
    
    [Test]
    public void TestContainedVariable()
    {
        var environment = new BakedEnvironment()
            .WithVariable("pizza", new MockPropertyObject());
        var session = CreateSession("a = pizza.foo").Init();
        session.ExecuteUntilEnd();

        var reference = new VariableReference(new[] { "pizza", "foo" }, session.Interpreter);

        Assert.True(reference.TryGetVariable(out var variable) && variable.Value.Equals("bar"));
    }

    private ScriptSession CreateSession(string text)
    {
        return new BakedEnvironment().CreateSession(new RawStringSource(text)).Init();
    }
    
    public class MockPropertyObject : BakedObject
    {
        public override object? GetValue()
        {
            return null;
        }

        public override bool TryGetContainedObject(string name, out BakedObject bakedObject)
        {
            bakedObject = new BakedNull();
            
            if (name == "foo")
            {
                bakedObject = new BakedString("bar");
                
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
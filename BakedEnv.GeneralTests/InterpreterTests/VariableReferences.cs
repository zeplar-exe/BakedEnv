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
        
        Assert.True(reference.VariableEquals("world"));
    }
    
    [Test]
    public void TestGetInvalid()
    {
        var session = CreateSession("foo = \"bar\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("baz", session.Interpreter);

        Assert.True(!reference.TryGetVariable(out var variable) && variable == null);
    }
    
    [Test]
    public void TestSet()
    {
        var session = CreateSession("foo = 50");
        session.ExecuteUntilEnd();

        var reference = new VariableReference("foo", session.Interpreter);
        
        Assert.True(reference.TrySetVariable(new BakedString("papa")) && reference.VariableEquals("papa"));
    }

    [Test]
    public void TestSetInvalid()
    {
        var session = CreateSession("");
        session.ExecuteUntilEnd();

        var reference = new VariableReference(new[] { "does", "not", "exist" }, session.Interpreter);

        Assert.False(reference.TrySetVariable(new BakedString("")));
    }
    
    [Test]
    public void TestContainedVariable()
    {
        var environment = new BakedEnvironment()
            .WithVariable("pizza", new MockPropertyObject());
        var session = environment.CreateSession(new RawStringSource("")).Init();
        session.ExecuteUntilEnd();

        var reference = new VariableReference(new[] { "pizza", "foo" }, session.Interpreter);

        Assert.True(reference.VariableEquals("bar"));
    }

    private ScriptSession CreateSession(string text)
    {
        return new BakedEnvironment().CreateSession(new RawStringSource(text)).Init();
    }
    
    public class MockPropertyObject : BakedObject
    {
        public const string PropertyName = "foo";
        public BakedString PropertyValue { get; set; }

        public MockPropertyObject()
        {
            PropertyValue = new BakedString("bar");
        }
        
        public override object? GetValue()
        {
            return null;
        }

        public override bool TryGetContainedObject(string name, out BakedObject bakedObject)
        {
            bakedObject = new BakedNull();
            
            if (name == PropertyName)
            {
                bakedObject = PropertyValue;
                
                return true;
            }

            return false;
        }

        public override bool TrySetContainedObject(string name, BakedObject bakedObject)
        {
            if (name == PropertyName)
            {
                if (bakedObject is not BakedString stringObject)
                    return false;

                PropertyValue = stringObject;

                return true;
            }

            return true;
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
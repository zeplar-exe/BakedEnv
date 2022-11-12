using BakedEnv.Environment;
using BakedEnv.Interpreter;
using BakedEnv.Objects;
using BakedEnv.Sources;
using BakedEnv.Variables;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class VariableReferences
{
    [Test]
    public void TestGet()
    {
        var session = InterpreterTestHelper.CreateSession("hello = \"world\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("hello", session.Interpreter);
        
        Assert.True(reference.VariableEquals("world"));
    }
    
    [Test]
    public void TestGetInvalid()
    {
        var session = InterpreterTestHelper.CreateSession("foo = \"bar\"");
        session.ExecuteUntilEnd();
        
        var reference = new VariableReference("baz", session.Interpreter);

        Assert.True(!reference.TryGetVariable(out var variable) && variable == null);
    }
    
    [Test]
    public void TestSet()
    {
        var session = InterpreterTestHelper.CreateSession("foo = 50");
        session.ExecuteUntilEnd();

        var reference = new VariableReference("foo", session.Interpreter);
        
        Assert.True(reference.TrySetVariable(new BakedString("papa")) && reference.VariableEquals("papa"));
    }

    [Test]
    public void TestSetInvalid()
    {
        var session = InterpreterTestHelper.CreateSession("");
        session.ExecuteUntilEnd();

        var reference = new VariableReference(new[] { "does", "not", "exist" }, session.Interpreter);

        Assert.False(reference.TrySetVariable(new BakedString("")));
    }
    
    [Test]
    public void TestContainedVariable()
    {
        var environment = new BakedEnvironmentBuilder()
            .WithVariable("pizza", new MockPropertyObject())
            .Build();
        var session = new ScriptSession(new BakedInterpreter(environment, new RawStringSource("")));
        session.ExecuteUntilEnd();

        var reference = new VariableReference(new[] { "pizza", "foo" }, session.Interpreter);

        Assert.True(reference.VariableEquals("bar"));
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
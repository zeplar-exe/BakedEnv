using BakedEnv.Interpreter.Sources;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Variables
{
    [Test]
    public void TestVariableAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("pizza = 1")).Init();
        session.ExecuteUntilEnd();

        Assert.True(session.Interpreter.Context.Variables["pizza"].Value.Equals(1));
    }

    [Test]
    public void TestMultipleVariableAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = 1 \n bar=2")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(session.Interpreter.Context.Variables["bar"].Value.Equals(2));
    }

    [Test]
    public void TestMultiAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = 1 \n foo=2")).Init();
        session.ExecuteUntilEnd();

        Assert.True(session.Interpreter.Context.Variables["foo"].Value.Equals(2));
    }
    
    [Test]
    public void TestReadOnlyVariable()
    {
        var environment = new BakedEnvironment()
            .WithVariable(new BakedVariable("Foo", new BakedInteger(1)) 
            {
                IsReadOnly = true 
            });
        var session = environment.CreateSession(new RawStringSource("Foo = 0")).Init();
        session.ExecuteUntilEnd();
        
        Assert.True(environment.GlobalVariables["Foo"].Value.Equals(1));
    }

    [Test]
    public void TestContainedVariable()
    {
        var environment = new BakedEnvironment()
            .WithVariable("pizza", new MockPropertyObject());
        var session = environment.CreateSession(new RawStringSource("a = pizza.foo")).Init();
        session.ExecuteUntilEnd();

        Assert.True(session.Interpreter.Context.Variables["a"].Value.Equals("bar"));
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
using BakedEnv.Environment;
using BakedEnv.Objects;
using BakedEnv.Sources;
using BakedEnv.Variables;

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

        session.AssertInterpreterHasVariable("pizza", 1);
    }

    [Test]
    public void TestMultipleVariableAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = 1 \n bar=2")).Init();
        session.ExecuteUntilEnd();
        
        session.AssertInterpreterHasVariable("bar", 2);
    }

    [Test]
    public void TestMultiAssignment()
    {
        var session = new BakedEnvironment().CreateSession(new RawStringSource("foo = 1 \n foo=2")).Init();
        session.ExecuteUntilEnd();
        
        session.AssertInterpreterHasVariable("foo", 2);
    }
    
    [Test]
    public void TestReadOnlyVariable()
    {
        var environment = new BakedEnvironmentBuilder()
            .WithVariable("Foo", new BakedInteger(1), VariableFlags.ReadOnly)
            .Build();
        var session = environment.CreateSession(new RawStringSource("Foo = 0")).Init();
        session.ExecuteUntilEnd();
        
        environment.AssertEnvironmentHasVariable("Foo", 1);
    }

    [Test]
    public void TestContainedVariable()
    {
        var environment = new BakedEnvironmentBuilder()
            .WithVariable("pizza", new MockPropertyObject())
            .Build();
        var session = environment.CreateSession(new RawStringSource("a = pizza.foo")).Init();
        session.ExecuteUntilEnd();

        session.AssertInterpreterHasVariable("a", "bar");
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
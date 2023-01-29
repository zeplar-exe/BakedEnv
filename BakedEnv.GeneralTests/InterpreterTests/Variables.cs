using BakedEnv.Environment;
using BakedEnv.Interpreter;
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
        var session = InterpreterTestHelper.CreateSession("pizza = 1");
        session.ExecuteUntilEnd();

        session.AssertInterpreterHasVariable("pizza", 1);
    }

    [Test]
    public void TestMultipleVariableAssignment()
    {
        var session = InterpreterTestHelper.CreateSession("foo = 1 \n bar = 2");
        session.ExecuteUntilEnd();
        
        session.AssertInterpreterHasVariable("bar", 2);
    }

    [Test]
    public void TestMultiAssignment()
    {
        var session = InterpreterTestHelper.CreateSession("foo = 1 \n foo = 2");
        session.ExecuteUntilEnd();
        
        session.AssertInterpreterHasVariable("foo", 2);
    }
    
    [Test]
    public void TestReadOnlyVariable()
    {
        var environment = new BakedEnvironmentBuilder()
            .WithVariable("Foo", new BakedInteger(1), VariableFlags.ReadOnly)
            .Build();
        var session = environment.CreateSession("Foo = 0");
        session.ExecuteUntilEnd();
        
        environment.AssertEnvironmentHasVariable("Foo", 1);
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
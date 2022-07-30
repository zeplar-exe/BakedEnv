using BakedEnv.Environment;
using BakedEnv.Objects;
using BakedEnv.Sources;

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

        var variable = session.TopVariables["foo"];
        var index = variable.Value.TryGetIndex(new[] { new BakedInteger(0) }, out var value);

        Assert.True(index && value.Equals("Bar"));
    }
}
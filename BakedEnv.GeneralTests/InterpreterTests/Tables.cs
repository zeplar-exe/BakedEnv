using BakedEnv.Objects;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class Tables
{
    [Test]
    public void TestTableDeclaration()
    {
        var session = InterpreterTestHelper.CreateSession("foo = [ 0 : \"Bar\" ]");
        session.ExecuteUntilError();

        var variable = session.TopVariables["foo"];
        var index = variable.Value.TryGetIndex(new[] { new BakedInteger(0) }, out var value);

        Assert.True(index && value.Equals("Bar"));
    }
}
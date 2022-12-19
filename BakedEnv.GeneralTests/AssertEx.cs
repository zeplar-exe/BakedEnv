using BakedEnv.Environment;
using BakedEnv.Interpreter;
using NUnit.Framework;

namespace BakedEnv.GeneralTests;

public static class AssertEx
{
    public static void AssertInterpreterHasVariable(this ScriptSession session, string name, object value)
    {
        session.Interpreter.AssertInterpreterHasVariable(name, value);
    }
    
    public static void AssertInterpreterHasVariable(this BakedInterpreter interpreter, string name, object value)
    {
        Assert.True(interpreter.Context.Variables[name].Value.Equals(value));
    }

    public static void AssertEnvironmentHasVariable(this BakedEnvironment environment, string name, object value)
    {
        Assert.True(environment.Variables[name].Value.Equals(value));
    }
}
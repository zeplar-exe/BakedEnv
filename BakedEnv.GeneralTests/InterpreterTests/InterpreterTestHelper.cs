using BakedEnv.Environment;
using BakedEnv.Interpreter;
using BakedEnv.Sources;

namespace BakedEnv.GeneralTests.InterpreterTests;

public static class InterpreterTestHelper
{
    public static ScriptSession CreateSession(this BakedEnvironment environment, string input)
    {
        return new ScriptSession(new BakedInterpreter(environment, new RawStringSource(input)));
    }
    
    public static ScriptSession CreateSession(string input)
    {
        return new ScriptSession(new BakedInterpreter(new RawStringSource(input)));
    }
}
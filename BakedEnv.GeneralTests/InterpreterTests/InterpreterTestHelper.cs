using System;

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
        var interpreter = new BakedInterpreter(new RawStringSource(input));

        #if DEBUG
        interpreter.Error.ErrorReported += (reporter, error) => Console.WriteLine(error.ToString());
        #endif
        
        return new ScriptSession(interpreter);
    }
}
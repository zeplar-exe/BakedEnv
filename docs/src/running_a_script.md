# Running a Script

The library provides a direct approach for executing scripts. That is, through the ScriptSession class. ScriptSession is, in essense, a helper class which wraps around a BakedInterpreter. In order to create a ScriptSession, a BakedInterpreter must be supplied, which in turn requires an IBakedSource.

```cs
IBakedSource source = new RawStringSource("my_script_var = 1;");
BakedInterpreter interpreter = new BakedInterpreter(source);
ScriptSession session = new ScriptSession(interpreter);

session.ExecuteUntilError(); // Executes parsed instructions until an error occurs
```

> AutoExecutionMode is an enum for one of two values, None and AfterYield, in ScriptSession, the value determines when an instruction is executed in EnumerateInstructions. In hindsight, this this effectively useless.

However, a BakedInterpreter can be used on its own without issue;

```cs
while (interpreter.TryGetNextInstruction(out InterpreterInstruction? instruction)
{
    instruction?.Execute(interpreter);
}
```

The interpreter itself is a required parameter for execution as to scope the information available to the instruction. The root scope is used in this case. Note that the source index is optional, but serves use in debugging and error messages.

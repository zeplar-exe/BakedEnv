using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Sources;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class ProcessorInstructions
{
    [Test]
    public void TestProcessorStatementParsing()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment().CreateSession(new RawStringSource("[BakeType: \"Cake\"]")).Init();
        session.ExecuteUntilEnd();

        Assert.True(processorStatement!.Value.Equals("Cake"));
    }
    
    [Test]
    public void TestProcessorStatementExecution()
    {
        var session = new BakedEnvironment()
            .WithStatementHandlers(new MockStatementHandler())
            .CreateSession(new RawStringSource($"[Pizza: \"Time\"]"))
            .Init();
        session.ExecuteUntilEnd();
        
        Assert.True(session.Interpreter.Context.Variables["Pizza"].Value.Equals("Time"));
    }

    [Test]
    public void TestWhitespace()
    {
        var session = new BakedEnvironment()
            .WithStatementHandlers(new MockStatementHandler())
            .CreateSession(new RawStringSource($"[    NaN: \t 0 \n ]"))
            .Init();
        session.ExecuteUntilEnd();

        Assert.True(session.Interpreter.Context.Variables["NaN"].Value.Equals(0));
    }

    private class MockStatementHandler : IProcessorStatementHandler
    {
        public bool TryHandle(ProcessorStatementInstruction instruction, BakedInterpreter interpreter)
        {
            interpreter.Context.Variables.Add(instruction.Name, instruction.Value);
            
            return true;
        }
    }
}